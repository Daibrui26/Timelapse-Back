using Microsoft.Data.SqlClient;
using TimelapseAPI.Models;

namespace TimelapseAPI.Repositories
{
    public class ContenidoRepository : IContenidoRepository
    {
        private readonly string _connectionString;

        public ContenidoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("TimelapseDB")
                ?? throw new Exception("Connection string not found");
        }

        private static Contenido Map(SqlDataReader r) => new Contenido
        {
            IdContenido    = r.GetInt32(0),
            Tipo           = r.GetString(1),
            ContenidoTexto = r.IsDBNull(2) ? null : r.GetString(2),
            UrlArchivo     = r.IsDBNull(3) ? null : r.GetString(3),
            PublicId       = r.IsDBNull(4) ? null : r.GetString(4),
            FechaSubida    = r.GetDateTime(5),
            IdCapsula      = r.GetInt32(6)
        };

        private const string SelectBase =
            "SELECT id_contenido, tipo, contenido, url_archivo, public_id, fecha_subida, id_capsula FROM Contenido";

        public async Task<List<Contenido>> GetAllAsync()
        {
            var list = new List<Contenido>();
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new SqlCommand(SelectBase, conn);
            using var r = await cmd.ExecuteReaderAsync();
            while (await r.ReadAsync()) list.Add(Map(r));
            return list;
        }

        public async Task<Contenido?> GetByIdAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new SqlCommand(SelectBase + " WHERE id_contenido = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            using var r = await cmd.ExecuteReaderAsync();
            return await r.ReadAsync() ? Map(r) : null;
        }

        public async Task<List<Contenido>> GetByCapsulaIdAsync(int idCapsula)
        {
            var list = new List<Contenido>();
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new SqlCommand(SelectBase + " WHERE id_capsula = @id ORDER BY fecha_subida DESC", conn);
            cmd.Parameters.AddWithValue("@id", idCapsula);
            using var r = await cmd.ExecuteReaderAsync();
            while (await r.ReadAsync()) list.Add(Map(r));
            return list;
        }

        public async Task<Contenido> CreateAsync(Contenido contenido)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            const string query = @"
                INSERT INTO Contenido (tipo, contenido, url_archivo, public_id, fecha_subida, id_capsula)
                OUTPUT INSERTED.id_contenido
                VALUES (@tipo, @contenido, @urlArchivo, @publicId, @fechaSubida, @idCapsula)";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@tipo",        contenido.Tipo);
            cmd.Parameters.AddWithValue("@contenido",   (object?)contenido.ContenidoTexto ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@urlArchivo",  (object?)contenido.UrlArchivo     ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@publicId",    (object?)contenido.PublicId       ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@fechaSubida", contenido.FechaSubida);
            cmd.Parameters.AddWithValue("@idCapsula",   contenido.IdCapsula);

            contenido.IdContenido = (int)await cmd.ExecuteScalarAsync();
            return contenido;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new SqlCommand("DELETE FROM Contenido WHERE id_contenido = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            return await cmd.ExecuteNonQueryAsync() > 0;
        }
    }
}