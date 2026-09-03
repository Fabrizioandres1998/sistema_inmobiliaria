// using InmobiliariaTPI.Models;
// using InmobiliariaTPI.Repositories;
// using Microsoft.Extensions.Logging;

// namespace InmobiliariaTPI.Services
// {
//     public class UsuarioService : BaseService<Usuario, IUsuarioRepository>, IUsuarioService
//     {
//         public UsuarioService(IUsuarioRepository repository, ILogger<Usuario> logger)
//             : base(repository, logger)
//         {
//         }

//         // busca un usuario por email
//         public async Task<Usuario?> GetByEmailAsync(string email)
//         {
//             _logger.LogInformation("Buscando usuario por email: {Email}", email);
//             return await _repository.GetByEmailAsync(email);
//         }

//         // verifica si ya existe un email registrado
//         public async Task<bool> ExisteEmailAsync(string email)
//         {
//             _logger.LogInformation("Verificando si existe email: {Email}", email);
//             return await _repository.ExisteEmailAsync(email);
//         }

//         // obtiene usuarios por rol
//         public async Task<IEnumerable<Usuario>> GetByRolAsync(RolUsuario rol)
//         {
//             _logger.LogInformation("Obteniendo usuarios con rol: {Rol}", rol);
//             return await _repository.GetByRolAsync(rol);
//         }

//         // valida las credenciales de un usuario
//         public async Task<bool> LoginAsync(string email, string password)
//         {
//             _logger.LogInformation("Intento de login para email: {Email}", email);
//             var usuario = await _repository.GetByEmailAsync(email);

//             if (usuario == null)
//             {
//                 _logger.LogWarning("Usuario no encontrado: {Email}", email);
//                 return false;
//             }

//             // comparacion simple (en produccion deberia ser con hash)
//             var success = usuario.Password == password;

//             if (success)
//             {
//                 _logger.LogInformation("Login exitoso para: {Email}", email);
//             }
//             else
//             {
//                 _logger.LogWarning("Contraseña incorrecta para: {Email}", email);
//             }

//             return success;
//         }

//         // crea un nuevo usuario validando que el email sea unico
//         public override async Task<Usuario> CreateAsync(Usuario usuario)
//         {
//             _logger.LogInformation("Creando nuevo usuario - Email: {Email}", usuario.Email);

//             if (await _repository.ExisteEmailAsync(usuario.Email!))
//                 throw new InvalidOperationException("Ya existe un usuario con ese email");

//             if (string.IsNullOrWhiteSpace(usuario.Password))
//                 throw new ArgumentException("La contraseña es obligatoria");

//             // en produccion, la contraseña deberia hashearse
//             usuario.FechaCreacion = DateTime.Now;

//             return await base.CreateAsync(usuario);
//         }

//         // actualiza un usuario validando el email
//         public override async Task UpdateAsync(Usuario usuario)
//         {
//             _logger.LogInformation("Actualizando usuario {Id}", usuario.Id);

//             var existente = await _repository.GetByIdAsync(usuario.Id);
//             if (existente == null)
//                 throw new InvalidOperationException("Usuario no encontrado");

//             // si cambia el email, valido que no este usado por otro
//             if (existente.Email != usuario.Email &&
//                 await _repository.ExisteEmailAsync(usuario.Email!))
//                 throw new InvalidOperationException("Ya existe otro usuario con ese email");

//             usuario.FechaUltimaModificacion = DateTime.Now;
//             await base.UpdateAsync(usuario);
//         }

//         // filtro para el paginado
//         protected override async Task<IEnumerable<Usuario>> SearchAsync(IEnumerable<Usuario> items, string searchTerm)
//         {
//             return items.Where(u =>
//                 u.NombreCompleto!.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
//                 u.Email!.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
//             );
//         }
//     }
// }