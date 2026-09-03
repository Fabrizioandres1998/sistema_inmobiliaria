// using InmobiliariaTPI.Data;
// using InmobiliariaTPI.Models;
// using MySql.Data.MySqlClient;
// using Microsoft.Extensions.Logging;

// namespace InmobiliariaTPI.Repositories
// {
//     public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
//     {
//         public UsuarioRepository(DatabaseHelper dbHelper, ILogger<Usuario> logger)
//             : base(dbHelper, logger)
//         {
//         }

//         public override async Task<IEnumerable<Usuario>> GetAllAsync()
//         {
//             _logger.LogInformation("Obteniendo todos los usuarios");
//             var usuarios = new List<Usuario>();
//             var query = "SELECT id_usuario, email, password, rol, nombre_completo, avatar, fecha_creacion, fecha_ultima_modificacion FROM usuario";

//             using (var reader = await _dbHelper.ExecuteReaderAsync(query))
//             {
//                 while (await reader.ReadAsync())
//                 {
//                     usuarios.Add(new Usuario
//                     {
//                         Id = reader.GetInt32(0),
//                         Email = reader.GetString(1),
//                         Password = reader.GetString(2),
//                         Rol = reader.GetString(3) == "Administrador" ? RolUsuario.Administrador : RolUsuario.Empleado,
//                         NombreCompleto = reader.GetString(4),
//                         Avatar = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
//                         FechaCreacion = reader.GetDateTime(6),
//                         FechaUltimaModificacion = reader.IsDBNull(7) ? (DateTime?)null : reader.GetDateTime(7)
//                     });
//                 }
//             }
//             _logger.LogInformation("Se obtuvieron {Count} usuarios", usuarios.Count);
//             return usuarios;
//         }

//         public override async Task<Usuario?> GetByIdAsync(int id)
//         {
//             _logger.LogInformation("Buscando usuario por ID: {Id}", id);
//             var query = "SELECT id_usuario, email, password, rol, nombre_completo, avatar, fecha_creacion, fecha_ultima_modificacion FROM usuario WHERE id_usuario = @Id";
//             var parameters = new MySqlParameter[] { new MySqlParameter("@Id", id) };

//             using (var reader = await _dbHelper.ExecuteReaderAsync(query, parameters))
//             {
//                 if (await reader.ReadAsync())
//                 {
//                     return new Usuario
//                     {
//                         Id = reader.GetInt32(0),
//                         Email = reader.GetString(1),
//                         Password = reader.GetString(2),
//                         Rol = reader.GetString(3) == "Administrador" ? RolUsuario.Administrador : RolUsuario.Empleado,
//                         NombreCompleto = reader.GetString(4),
//                         Avatar = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
//                         FechaCreacion = reader.GetDateTime(6),
//                         FechaUltimaModificacion = reader.IsDBNull(7) ? (DateTime?)null : reader.GetDateTime(7)
//                     };
//                 }
//                 _logger.LogWarning("Usuario con ID: {Id} no encontrado", id);
//                 return null;
//             }
//         }

//         public override async Task<int> CreateAsync(Usuario usuario)
//         {
//             _logger.LogInformation("Creando nuevo usuario - Email: {Email}", usuario.Email);
//             var query = @"INSERT INTO usuario (email, password, rol, nombre_completo, avatar, fecha_creacion) 
//                         VALUES (@Email, @Password, @Rol, @NombreCompleto, @Avatar, @FechaCreacion);
//                         SELECT LAST_INSERT_ID();";

//             var parameters = new MySqlParameter[]
//             {
//                 new MySqlParameter("@Email", usuario.Email),
//                 new MySqlParameter("@Password", usuario.Password),
//                 new MySqlParameter("@Rol", usuario.Rol.ToString()),
//                 new MySqlParameter("@NombreCompleto", usuario.NombreCompleto),
//                 new MySqlParameter("@Avatar", string.IsNullOrEmpty(usuario.Avatar) ? (object)DBNull.Value : usuario.Avatar),
//                 new MySqlParameter("@FechaCreacion", usuario.FechaCreacion == default ? DateTime.Now : usuario.FechaCreacion)
//             };

//             var result = await _dbHelper.ExecuteScalarAsync(query, parameters);
//             var id = result != null ? Convert.ToInt32(result) : 0;
//             _logger.LogInformation("Usuario creado con ID: {Id}", id);
//             return id;
//         }

//         public override async Task UpdateAsync(Usuario usuario)
//         {
//             _logger.LogInformation("Actualizando usuario ID: {Id}", usuario.Id);
//             var query = @"UPDATE usuario 
//                         SET email = @Email, 
//                             password = @Password, 
//                             rol = @Rol, 
//                             nombre_completo = @NombreCompleto, 
//                             avatar = @Avatar,
//                             fecha_ultima_modificacion = @FechaUltimaModificacion
//                         WHERE id_usuario = @Id";

//             var parameters = new MySqlParameter[]
//             {
//                 new MySqlParameter("@Id", usuario.Id),
//                 new MySqlParameter("@Email", usuario.Email),
//                 new MySqlParameter("@Password", usuario.Password),
//                 new MySqlParameter("@Rol", usuario.Rol.ToString()),
//                 new MySqlParameter("@NombreCompleto", usuario.NombreCompleto),
//                 new MySqlParameter("@Avatar", string.IsNullOrEmpty(usuario.Avatar) ? (object)DBNull.Value : usuario.Avatar),
//                 new MySqlParameter("@FechaUltimaModificacion", DateTime.Now)
//             };

//             await _dbHelper.ExecuteNonQueryAsync(query, parameters);
//             _logger.LogInformation("Usuario ID: {Id} actualizado correctamente", usuario.Id);
//         }

//         public override async Task DeleteAsync(int id)
//         {
//             _logger.LogInformation("Eliminando usuario ID: {Id}", id);
//             var query = "DELETE FROM usuario WHERE id_usuario = @Id";
//             var parameters = new MySqlParameter[] { new MySqlParameter("@Id", id) };
//             await _dbHelper.ExecuteNonQueryAsync(query, parameters);
//             _logger.LogInformation("Usuario ID: {Id} eliminado correctamente", id);
//         }

//         public async Task<Usuario?> GetByEmailAsync(string email)
//         {
//             _logger.LogInformation("Buscando usuario por email: {Email}", email);
//             var query = "SELECT id_usuario, email, password, rol, nombre_completo, avatar, fecha_creacion, fecha_ultima_modificacion FROM usuario WHERE email = @Email";
//             var parameters = new MySqlParameter[] { new MySqlParameter("@Email", email) };

//             using (var reader = await _dbHelper.ExecuteReaderAsync(query, parameters))
//             {
//                 if (await reader.ReadAsync())
//                 {
//                     return new Usuario
//                     {
//                         Id = reader.GetInt32(0),
//                         Email = reader.GetString(1),
//                         Password = reader.GetString(2),
//                         Rol = reader.GetString(3) == "Administrador" ? RolUsuario.Administrador : RolUsuario.Empleado,
//                         NombreCompleto = reader.GetString(4),
//                         Avatar = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
//                         FechaCreacion = reader.GetDateTime(6),
//                         FechaUltimaModificacion = reader.IsDBNull(7) ? (DateTime?)null : reader.GetDateTime(7)
//                     };
//                 }
//                 _logger.LogWarning("Usuario con email: {Email} no encontrado", email);
//                 return null;
//             }
//         }

//         public async Task<bool> ExisteEmailAsync(string email)
//         {
//             _logger.LogInformation("Verificando si existe email: {Email}", email);
//             var query = "SELECT COUNT(1) FROM usuario WHERE email = @Email";
//             var parameters = new MySqlParameter[] { new MySqlParameter("@Email", email) };
//             var result = await _dbHelper.ExecuteScalarAsync(query, parameters);
//             var count = result != null ? Convert.ToInt32(result) : 0;
//             _logger.LogInformation("Email: {Email} existe: {Existe}", email, count > 0);
//             return count > 0;
//         }

//         public async Task<IEnumerable<Usuario>> GetByRolAsync(RolUsuario rol)
//         {
//             _logger.LogInformation("Obteniendo usuarios con rol: {Rol}", rol);
//             var usuarios = new List<Usuario>();
//             var query = "SELECT id_usuario, email, password, rol, nombre_completo, avatar, fecha_creacion, fecha_ultima_modificacion FROM usuario WHERE rol = @Rol";
//             var parameters = new MySqlParameter[] { new MySqlParameter("@Rol", rol.ToString()) };

//             using (var reader = await _dbHelper.ExecuteReaderAsync(query, parameters))
//             {
//                 while (await reader.ReadAsync())
//                 {
//                     usuarios.Add(new Usuario
//                     {
//                         Id = reader.GetInt32(0),
//                         Email = reader.GetString(1),
//                         Password = reader.GetString(2),
//                         Rol = reader.GetString(3) == "Administrador" ? RolUsuario.Administrador : RolUsuario.Empleado,
//                         NombreCompleto = reader.GetString(4),
//                         Avatar = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
//                         FechaCreacion = reader.GetDateTime(6),
//                         FechaUltimaModificacion = reader.IsDBNull(7) ? (DateTime?)null : reader.GetDateTime(7)
//                     });
//                 }
//             }
//             _logger.LogInformation("Se obtuvieron {Count} usuarios con rol {Rol}", usuarios.Count, rol);
//             return usuarios;
//         }
//     }
// }