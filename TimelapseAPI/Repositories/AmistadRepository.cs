using System.Data.SqlClient;
using TimelapseAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace TimelapseAPI.Repositories
{

    public class AmistadRepository : IAmistadRepository
    {
        private readonly string _connectionString;

        public AmistadRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("TimelapseDB") ?? throw new Exception("Connection string not found");
        }

        public async Task<List<Amistad>> GetAllAsync()
        {
            var list = new List<Amistad>();
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string query = "SELECT id_amistad, id_usuario1, id_usuario2, estado FROM Amistad";
            using var cmd = new SqlCommand(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new Amistad
                {
                    IdAmistad = reader.GetInt32(0),
                    IdUsuario1 = reader.GetInt32(1),
                    IdUsuario2 = reader.GetInt32(2),
                    Estado = reader.GetString(3)
                });
            }
            return list;
        }

        public async Task<Amistad?> GetByIdAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string query = "SELECT id_amistad, id_usuario1, id_usuario2, estado FROM Amistad WHERE id_amistad=@id";
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Amistad
                {
                    IdAmistad = reader.GetInt32(0),
                    IdUsuario1 = reader.GetInt32(1),
                    IdUsuario2 = reader.GetInt32(2),
                    Estado = reader.GetString(3)
                };
            }
            return null;
        }

        public async Task<Amistad> CreateAsync(Amistad amistad)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string query = @"INSERT INTO Amistad (id_usuario1, id_usuario2, estado) 
                             OUTPUT INSERTED.id_amistad 
                             VALUES (@idUsuario1, @idUsuario2, @estado)";
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@idUsuario1", amistad.IdUsuario1);
            cmd.Parameters.AddWithValue("@idUsuario2", amistad.IdUsuario2);
            cmd.Parameters.AddWithValue("@estado", amistad.Estado);

            amistad.IdAmistad = (int)await cmd.ExecuteScalarAsync();
            return amistad;
        }

        public async Task<Amistad?> UpdateAsync(Amistad amistad)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string query = @"UPDATE Amistad 
                             SET id_usuario1=@idUsuario1, id_usuario2=@idUsuario2, estado=@estado 
                             WHERE id_amistad=@id";
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", amistad.IdAmistad);
            cmd.Parameters.AddWithValue("@idUsuario1", amistad.IdUsuario1);
            cmd.Parameters.AddWithValue("@idUsuario2", amistad.IdUsuario2);
            cmd.Parameters.AddWithValue("@estado", amistad.Estado);

            int rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0 ? amistad : null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string query = "DELETE FROM Amistad WHERE id_amistad=@id";
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        
    }
}