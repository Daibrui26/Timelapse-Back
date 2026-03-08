using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using TimelapseAPI.Models;

namespace TimelapseAPI.Repositories
{

    public class NotificacionRepository : INotificacionRepository
    {
        private readonly string _connectionString;

        public NotificacionRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("TimelapseDB") 
                ?? throw new Exception("Connection string not found");
        }

        public async Task<List<Notificacion>> GetAllAsync()
        {
            var list = new List<Notificacion>();
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string query = "SELECT id_notificacion, tipo, mensaje, fecha_creacion, leida, id_usuario, id_capsula FROM Notificacion";
            using var cmd = new SqlCommand(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new Notificacion
                {
                    IdNotificacion = reader.GetInt32(0),
                    Tipo = reader.GetString(1),
                    Mensaje = reader.GetString(2),
                    FechaCreacion = reader.GetDateTime(3),
                    Leida = reader.GetBoolean(4),
                    IdUsuario = reader.GetInt32(5),
                    IdCapsula = reader.IsDBNull(6) ? null : reader.GetInt32(6)
                });
            }

            return list;
        }

        public async Task<Notificacion?> GetByIdAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string query = "SELECT id_notificacion, tipo, mensaje, fecha_creacion, leida, id_usuario, id_capsula FROM Notificacion WHERE id_notificacion=@id";
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Notificacion
                {
                    IdNotificacion = reader.GetInt32(0),
                    Tipo = reader.GetString(1),
                    Mensaje = reader.GetString(2),
                    FechaCreacion = reader.GetDateTime(3),
                    Leida = reader.GetBoolean(4),
                    IdUsuario = reader.GetInt32(5),
                    IdCapsula = reader.IsDBNull(6) ? null : reader.GetInt32(6)
                };
            }

            return null;
        }

        public async Task<Notificacion> CreateAsync(Notificacion notificacion)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string query = @"
                INSERT INTO Notificacion (tipo, mensaje, fecha_creacion, leida, id_usuario, id_capsula)
                OUTPUT INSERTED.id_notificacion
                VALUES (@tipo, @mensaje, @fechaCreacion, @leida, @idUsuario, @idCapsula)";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@tipo", notificacion.Tipo);
            cmd.Parameters.AddWithValue("@mensaje", notificacion.Mensaje);
            cmd.Parameters.AddWithValue("@fechaCreacion", notificacion.FechaCreacion);
            cmd.Parameters.AddWithValue("@leida", notificacion.Leida ? 1 : 0);
            cmd.Parameters.AddWithValue("@idUsuario", notificacion.IdUsuario);
            cmd.Parameters.AddWithValue("@idCapsula", (object?)notificacion.IdCapsula ?? DBNull.Value);

            notificacion.IdNotificacion = (int)await cmd.ExecuteScalarAsync();
            return notificacion;
        }

        public async Task<Notificacion?> UpdateAsync(Notificacion notificacion)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string query = @"
                UPDATE Notificacion
                SET tipo=@tipo, mensaje=@mensaje, fecha_creacion=@fechaCreacion, leida=@leida, id_usuario=@idUsuario, id_capsula=@idCapsula
                WHERE id_notificacion=@id";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", notificacion.IdNotificacion);
            cmd.Parameters.AddWithValue("@tipo", notificacion.Tipo);
            cmd.Parameters.AddWithValue("@mensaje", notificacion.Mensaje);
            cmd.Parameters.AddWithValue("@fechaCreacion", notificacion.FechaCreacion);
            cmd.Parameters.AddWithValue("@leida", notificacion.Leida ? 1 : 0);
            cmd.Parameters.AddWithValue("@idUsuario", notificacion.IdUsuario);
            cmd.Parameters.AddWithValue("@idCapsula", (object?)notificacion.IdCapsula ?? DBNull.Value);

            int rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0 ? notificacion : null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string query = "DELETE FROM Notificacion WHERE id_notificacion=@id";
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }
    }
}