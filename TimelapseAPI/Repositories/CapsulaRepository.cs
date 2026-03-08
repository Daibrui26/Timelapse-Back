using TimelapseAPI.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace TimelapseAPI.Repositories
{
    public class CapsulaRepository : ICapsulaRepository
    {
        private readonly string _connectionString;

        public CapsulaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("TimelapseDB") ?? throw new Exception("Connection string no encontrada");
        }

        // GET ALL
        public async Task<List<Capsula>> GetAllAsync()
        {
            var capsulas = new List<Capsula>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT id_capsula, titulo, descripcion, fecha_creacion, fecha_apertura, estado, visibilidad FROM Capsula";

                using (var command = new SqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        capsulas.Add(MapCapsula(reader));
                    }
                }
            }

            return capsulas;
        }

        // GET BY ID
        public async Task<Capsula?> GetByIdAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT id_capsula, titulo, descripcion, fecha_creacion, fecha_apertura, estado, visibilidad FROM Capsula WHERE id_capsula=@Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return MapCapsula(reader);
                        }
                    }
                }
            }

            return null;
        }

        // CREATE
        public async Task CreateAsync(Capsula capsula)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"INSERT INTO Capsula (titulo, descripcion, fecha_creacion, fecha_apertura, estado, visibilidad) 
                                 VALUES (@Titulo, @Descripcion, @FechaCreacion, @FechaApertura, @Estado, @Visibilidad);
                                 SELECT SCOPE_IDENTITY();";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Titulo", capsula.Titulo);
                    command.Parameters.AddWithValue("@Descripcion", capsula.Descripcion);
                    command.Parameters.AddWithValue("@FechaCreacion", capsula.FechaCreacion);
                    command.Parameters.AddWithValue("@FechaApertura", capsula.FechaApertura);
                    command.Parameters.AddWithValue("@Estado", capsula.Estado);
                    command.Parameters.AddWithValue("@Visibilidad", capsula.Visibilidad);

                    var result = await command.ExecuteScalarAsync();
                    capsula.IdCapsula = Convert.ToInt32(result);
                }
            }
        }

        // UPDATE
        public async Task<Capsula?> UpdateAsync(Capsula capsula)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"UPDATE Capsula 
                                 SET titulo=@Titulo, descripcion=@Descripcion, fecha_creacion=@FechaCreacion, fecha_apertura=@FechaApertura, estado=@Estado, visibilidad=@Visibilidad
                                 WHERE id_capsula=@Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", capsula.IdCapsula);
                    command.Parameters.AddWithValue("@Titulo", capsula.Titulo);
                    command.Parameters.AddWithValue("@Descripcion", capsula.Descripcion);
                    command.Parameters.AddWithValue("@FechaCreacion", capsula.FechaCreacion);
                    command.Parameters.AddWithValue("@FechaApertura", capsula.FechaApertura);
                    command.Parameters.AddWithValue("@Estado", capsula.Estado);
                    command.Parameters.AddWithValue("@Visibilidad", capsula.Visibilidad);

                    var rows = await command.ExecuteNonQueryAsync();
                    if (rows == 0) return null;

                    return capsula;
                }
            }
        }

        // DELETE
        public async Task<bool> DeleteAsync(int id)
{
    using (var connection = new SqlConnection(_connectionString))
    {
        await connection.OpenAsync();
        using var transaction = connection.BeginTransaction();

        try
        {
            // 1. Borrar contenido
            using (var cmd = new SqlCommand(
                "DELETE FROM Contenido WHERE id_capsula = @Id", connection, transaction))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                await cmd.ExecuteNonQueryAsync();
            }

            // 2. Borrar comentarios
            using (var cmd = new SqlCommand(
                "DELETE FROM Comentario WHERE id_capsula = @Id", connection, transaction))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                await cmd.ExecuteNonQueryAsync();
            }

            // 3. Borrar notificaciones
            using (var cmd = new SqlCommand(
                "DELETE FROM Notificacion WHERE id_capsula = @Id", connection, transaction))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                await cmd.ExecuteNonQueryAsync();
            }

            // 4. Borrar participantes
            using (var cmd = new SqlCommand(
                "DELETE FROM Usuario_Capsula WHERE id_capsula = @Id", connection, transaction))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                await cmd.ExecuteNonQueryAsync();
            }

            // 5. Borrar la cápsula
            int rows;
            using (var cmd = new SqlCommand(
                "DELETE FROM Capsula WHERE id_capsula = @Id", connection, transaction))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                rows = await cmd.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();
            return rows > 0;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}

        // GET ALL FILTERED + ORDER
        public async Task<List<Capsula>> GetAllFilteredAsync(string? titulo, string? estado, string? orderBy, bool ascending)
        {
            var capsulas = new List<Capsula>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT id_capsula, titulo, descripcion, fecha_creacion, fecha_apertura, estado, visibilidad FROM Capsula WHERE 1=1";
                var parameters = new List<SqlParameter>();

                if (!string.IsNullOrWhiteSpace(titulo))
                {
                    query += " AND titulo LIKE @Titulo";
                    parameters.Add(new SqlParameter("@Titulo", $"%{titulo}%"));
                }

                if (!string.IsNullOrWhiteSpace(estado))
                {
                    query += " AND estado=@Estado";
                    parameters.Add(new SqlParameter("@Estado", estado));
                }

                // Ordenación
                if (!string.IsNullOrWhiteSpace(orderBy))
                {
                    var validColumns = new[] { "id_capsula", "titulo", "fecha_creacion", "fecha_apertura", "estado", "visibilidad" };
                    var orderByLower = orderBy.ToLower();

                    if (validColumns.Contains(orderByLower))
                    {
                        var direction = ascending ? "ASC" : "DESC";
                        query += $" ORDER BY {orderByLower} {direction}";
                    }
                    else
                    {
                        query += " ORDER BY titulo ASC";
                    }
                }
                else
                {
                    query += " ORDER BY titulo ASC";
                }

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddRange(parameters.ToArray());

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            capsulas.Add(MapCapsula(reader));
                        }
                    }
                }
            }

            return capsulas;
        }

        // GET BY USUARIO
public async Task<List<Capsula>> GetByUsuarioAsync(int idUsuario)
{
    var capsulas = new List<Capsula>();

    using (var connection = new SqlConnection(_connectionString))
    {
        await connection.OpenAsync();
        string query = @"
            SELECT c.id_capsula, c.titulo, c.descripcion, c.fecha_creacion, c.fecha_apertura, c.estado, c.visibilidad
            FROM Capsula c
            INNER JOIN Usuario_Capsula uc ON c.id_capsula = uc.id_capsula
            WHERE uc.id_usuario = @IdUsuario";

        using (var command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@IdUsuario", idUsuario);

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    capsulas.Add(MapCapsula(reader));
                }
            }
        }
    }

    return capsulas;
}

        // Helper para mapear SqlDataReader a Capsula
        private Capsula MapCapsula(SqlDataReader reader)
        {
            return new Capsula
            {
                IdCapsula = reader.GetInt32(0),
                Titulo = reader.GetString(1),
                Descripcion = reader.GetString(2),
                FechaCreacion = reader.GetDateTime(3),
                FechaApertura = reader.GetDateTime(4),
                Estado = reader.GetString(5),
                Visibilidad = reader.GetString(6)
            };
        }
    }
}