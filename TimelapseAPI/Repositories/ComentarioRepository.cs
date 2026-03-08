using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using TimelapseAPI.Models;

namespace TimelapseAPI.Repositories
{

    public class ComentarioRepository : IComentarioRepository
    {
        private readonly string _connectionString;

        public ComentarioRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("TimelapseDB") 
                ?? throw new Exception("Connection string not found");
        }

        public async Task<List<Comentario>> GetAllAsync()
        {
            var list = new List<Comentario>();
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string query = "SELECT id_comentario, texto, fecha_comentario, id_usuario, id_capsula FROM Comentario";
            using var cmd = new SqlCommand(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new Comentario
                {
                    IdComentario = reader.GetInt32(0),
                    Texto = reader.GetString(1),
                    FechaComentario = reader.GetDateTime(2),
                    IdUsuario = reader.GetInt32(3),
                    IdCapsula = reader.GetInt32(4)
                });
            }

            return list;
        }

        public async Task<Comentario?> GetByIdAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string query = "SELECT id_comentario, texto, fecha_comentario, id_usuario, id_capsula FROM Comentario WHERE id_comentario=@id";
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Comentario
                {
                    IdComentario = reader.GetInt32(0),
                    Texto = reader.GetString(1),
                    FechaComentario = reader.GetDateTime(2),
                    IdUsuario = reader.GetInt32(3),
                    IdCapsula = reader.GetInt32(4)
                };
            }

            return null;
        }

        public async Task<Comentario> CreateAsync(Comentario comentario)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string query = @"
                INSERT INTO Comentario (texto, fecha_comentario, id_usuario, id_capsula)
                OUTPUT INSERTED.id_comentario
                VALUES (@texto, @fechaComentario, @idUsuario, @idCapsula)";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@texto", comentario.Texto);
            cmd.Parameters.AddWithValue("@fechaComentario", comentario.FechaComentario);
            cmd.Parameters.AddWithValue("@idUsuario", comentario.IdUsuario);
            cmd.Parameters.AddWithValue("@idCapsula", comentario.IdCapsula);

            comentario.IdComentario = (int)await cmd.ExecuteScalarAsync();
            return comentario;
        }

        public async Task<Comentario?> UpdateAsync(Comentario comentario)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string query = @"
                UPDATE Comentario
                SET texto=@texto, fecha_comentario=@fechaComentario, id_usuario=@idUsuario, id_capsula=@idCapsula
                WHERE id_comentario=@id";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", comentario.IdComentario);
            cmd.Parameters.AddWithValue("@texto", comentario.Texto);
            cmd.Parameters.AddWithValue("@fechaComentario", comentario.FechaComentario);
            cmd.Parameters.AddWithValue("@idUsuario", comentario.IdUsuario);
            cmd.Parameters.AddWithValue("@idCapsula", comentario.IdCapsula);

            int rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0 ? comentario : null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string query = "DELETE FROM Comentario WHERE id_comentario=@id";
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<List<Comentario>> GetByCapsulaAsync(int idCapsula)
        {
            var list = new List<Comentario>();
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string query = @"SELECT id_comentario, texto, fecha_comentario, id_usuario, id_capsula 
                            FROM Comentario 
                            WHERE id_capsula = @idCapsula 
                            ORDER BY fecha_comentario DESC";
         using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@idCapsula", idCapsula);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new Comentario
                {
                 IdComentario = reader.GetInt32(0),
                 Texto = reader.GetString(1),
                 FechaComentario = reader.GetDateTime(2),
                 IdUsuario = reader.GetInt32(3),
                 IdCapsula = reader.GetInt32(4)
                });
            }
            return list;
        }
    }
}