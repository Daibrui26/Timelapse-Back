using TimelapseAPI.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TimelapseAPI.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("TimelapseDB") ?? throw new Exception("Connection string no encontrada");
        }

        // GET ALL
        public async Task<List<Usuario>> GetAllAsync()
        {
            var usuarios = new List<Usuario>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT id_usuario, nombre, email, contraseña, rol FROM Usuario";

                using (var command = new SqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        usuarios.Add(new Usuario
                        {
                            IdUsuario = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Email = reader.GetString(2),
                            Contraseña = reader.GetString(3),
                            Rol = reader.GetString(4)
                        });
                    }
                }
            }

            return usuarios;
        }

        // GET BY ID
        public async Task<Usuario?> GetByIdAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT id_usuario, nombre, email, contraseña, rol FROM Usuario WHERE id_usuario = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Usuario
                            {
                                IdUsuario = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Email = reader.GetString(2),
                                Contraseña = reader.GetString(3),
                                Rol = reader.GetString(4)
                            };
                        }
                    }
                }
            }

            return null;
        }

        // GET BY EMAIL
        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT id_usuario, nombre, email, contraseña, rol FROM Usuario WHERE email = @Email";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Usuario
                            {
                                IdUsuario = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Email = reader.GetString(2),
                                Contraseña = reader.GetString(3),
                                Rol = reader.GetString(4)
                            };
                        }
                    }
                }
            }

            return null;
        }

        // GET ALL FILTERED + ORDER
        public async Task<List<Usuario>> GetAllFilteredAsync(string? nombre, string? email, string? orderBy, bool ascending)
        {
            var usuarios = new List<Usuario>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT id_usuario, nombre, email, contraseña, rol FROM Usuario WHERE 1=1";
                var parameters = new List<SqlParameter>();

                if (!string.IsNullOrWhiteSpace(nombre))
                {
                    query += " AND nombre LIKE @Nombre";
                    parameters.Add(new SqlParameter("@Nombre", $"%{nombre}%"));
                }

                if (!string.IsNullOrWhiteSpace(email))
                {
                    query += " AND email LIKE @Email";
                    parameters.Add(new SqlParameter("@Email", $"%{email}%"));
                }

                if (!string.IsNullOrWhiteSpace(orderBy))
                {
                    var validColumns = new[] { "id_usuario", "nombre", "email" };
                    var orderByLower = orderBy.ToLower();

                    if (validColumns.Contains(orderByLower))
                    {
                        var direction = ascending ? "ASC" : "DESC";
                        query += $" ORDER BY {orderByLower} {direction}";
                    }
                    else
                    {
                        query += " ORDER BY nombre ASC";
                    }
                }
                else
                {
                    query += " ORDER BY nombre ASC";
                }

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddRange(parameters.ToArray());

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            usuarios.Add(new Usuario
                            {
                                IdUsuario = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Email = reader.GetString(2),
                                Contraseña = reader.GetString(3),
                                Rol = reader.GetString(4)
                            });
                        }
                    }
                }
            }

            return usuarios;
        }

        // CREATE
        public async Task CreateAsync(Usuario usuario)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "INSERT INTO Usuario (nombre, email, contraseña, rol) VALUES (@Nombre, @Email, @Contraseña, @Rol); SELECT SCOPE_IDENTITY();";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    command.Parameters.AddWithValue("@Email", usuario.Email);
                    command.Parameters.AddWithValue("@Contraseña", usuario.Contraseña);
                    command.Parameters.AddWithValue("@Rol", usuario.Rol);
                    
                    var result = await command.ExecuteScalarAsync();
                    usuario.IdUsuario = Convert.ToInt32(result);
                }
            }
        }

        // UPDATE
        public async Task<Usuario?> UpdateAsync(Usuario usuario)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "UPDATE Usuario SET nombre=@Nombre, email=@Email, contraseña=@Contraseña, rol=@Rol WHERE id_usuario=@Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", usuario.IdUsuario);
                    command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    command.Parameters.AddWithValue("@Email", usuario.Email);
                    command.Parameters.AddWithValue("@Contraseña", usuario.Contraseña);
                    command.Parameters.AddWithValue("@Rol", usuario.Rol);

                    var rows = await command.ExecuteNonQueryAsync();
                    if (rows == 0) return null;

                    return usuario;
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
            // 1. Borrar comentarios del usuario
            using (var cmd = new SqlCommand(
                "DELETE FROM Comentario WHERE id_usuario = @Id", connection, transaction))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                await cmd.ExecuteNonQueryAsync();
            }

            // 2. Borrar notificaciones del usuario
            using (var cmd = new SqlCommand(
                "DELETE FROM Notificacion WHERE id_usuario = @Id", connection, transaction))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                await cmd.ExecuteNonQueryAsync();
            }

            // 3. Borrar amistades del usuario
            using (var cmd = new SqlCommand(
                "DELETE FROM Amistad WHERE id_usuario1 = @Id OR id_usuario2 = @Id", connection, transaction))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                await cmd.ExecuteNonQueryAsync();
            }

            // 4. Borrar participaciones en cápsulas
            using (var cmd = new SqlCommand(
                "DELETE FROM Usuario_Capsula WHERE id_usuario = @Id", connection, transaction))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                await cmd.ExecuteNonQueryAsync();
            }

            // 5. Borrar el usuario
            int rows;
            using (var cmd = new SqlCommand(
                "DELETE FROM Usuario WHERE id_usuario = @Id", connection, transaction))
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
    }
}