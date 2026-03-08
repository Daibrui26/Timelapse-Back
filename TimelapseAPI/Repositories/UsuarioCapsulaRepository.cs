using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TimelapseAPI.Models;

namespace TimelapseAPI.Repositories
{
    public class UsuarioCapsulaRepository : IUsuarioCapsulaRepository
    {
        private readonly string _connectionString;

        public UsuarioCapsulaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("TimelapseDB")
                ?? throw new Exception("Connection string not found");
        }

        public async Task<List<UsuarioCapsula>> GetAllAsync()
        {
            var list = new List<UsuarioCapsula>();
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string query = "SELECT id_usuario_capsula, id_usuario, id_capsula, rol FROM Usuario_Capsula";
            using var cmd = new SqlCommand(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
                list.Add(Map(reader));

            return list;
        }

        public async Task<UsuarioCapsula?> GetByIdAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string query = "SELECT id_usuario_capsula, id_usuario, id_capsula, rol FROM Usuario_Capsula WHERE id_usuario_capsula = @id";
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync() ? Map(reader) : null;
        }

        public async Task<List<UsuarioCapsula>> GetByCapsulaAsync(int idCapsula)
        {
            var list = new List<UsuarioCapsula>();
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string query = "SELECT id_usuario_capsula, id_usuario, id_capsula, rol FROM Usuario_Capsula WHERE id_capsula = @idCapsula";
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@idCapsula", idCapsula);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                list.Add(Map(reader));

            return list;
        }

        public async Task<UsuarioCapsula> CreateAsync(UsuarioCapsula usuarioCapsula)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string query = @"INSERT INTO Usuario_Capsula (id_usuario, id_capsula, rol)
                             OUTPUT INSERTED.id_usuario_capsula
                             VALUES (@idUsuario, @idCapsula, @rol)";
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@idUsuario", usuarioCapsula.IdUsuario);
            cmd.Parameters.AddWithValue("@idCapsula", usuarioCapsula.IdCapsula);
            cmd.Parameters.AddWithValue("@rol", usuarioCapsula.Rol);

            usuarioCapsula.IdUsuarioCapsula = (int)await cmd.ExecuteScalarAsync();
            return usuarioCapsula;
        }

        public async Task<UsuarioCapsula?> UpdateAsync(UsuarioCapsula usuarioCapsula)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string query = @"UPDATE Usuario_Capsula
                             SET id_usuario = @idUsuario, id_capsula = @idCapsula, rol = @rol
                             WHERE id_usuario_capsula = @id";
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", usuarioCapsula.IdUsuarioCapsula);
            cmd.Parameters.AddWithValue("@idUsuario", usuarioCapsula.IdUsuario);
            cmd.Parameters.AddWithValue("@idCapsula", usuarioCapsula.IdCapsula);
            cmd.Parameters.AddWithValue("@rol", usuarioCapsula.Rol);

            int rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0 ? usuarioCapsula : null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string query = "DELETE FROM Usuario_Capsula WHERE id_usuario_capsula = @id";
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        private static UsuarioCapsula Map(SqlDataReader r) => new UsuarioCapsula
        {
            IdUsuarioCapsula = r.GetInt32(0),
            IdUsuario        = r.GetInt32(1),
            IdCapsula        = r.GetInt32(2),
            Rol              = r.GetString(3)
        };
    }
}