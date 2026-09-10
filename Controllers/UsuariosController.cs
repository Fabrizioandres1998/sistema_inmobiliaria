using InmobiliariaTPI.Models;
using InmobiliariaTPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InmobiliariaTPI.Controllers
{
    [Authorize] // Cualquier usuario logueado
    public class UsuariosController : BaseController
    {
        private readonly IUsuarioService _service;

        public UsuariosController(IUsuarioService service, ILogger<UsuariosController> logger)
            : base(logger)
        {
            _service = service;
        }

        // ============================================================
        // ACCIONES DE ADMIN (solo rol Administrador)
        // ============================================================

        // GET: Usuario
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? searchTerm = null)
        {
            _logger.LogInformation("Obteniendo lista de usuarios - Pagina: {Page}, Busqueda: {SearchTerm}", page, searchTerm ?? "ninguna");
            ViewBag.SearchTerm = searchTerm;
            var usuarios = await _service.GetPagedAsync(page, pageSize, searchTerm);
            return View(usuarios);
        }

        // GET: Usuario/Details/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation("Obteniendo detalle del usuario ID: {Id}", id);
            var usuario = await _service.GetByIdAsync(id);
            if (usuario == null)
            {
                _logger.LogWarning("Usuario ID: {Id} no encontrado", id);
                return NotFound();
            }
            return View(usuario);
        }

        // GET: Usuario/Create
        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            _logger.LogInformation("Mostrando formulario de creacion de usuario");
            return View();
        }

        // POST: Usuario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState invalido al crear usuario");
                return View(usuario);
            }

            try
            {
                _logger.LogInformation("Creando nuevo usuario - Email: {Email}", usuario.Email);
                await _service.CreateAsync(usuario);
                SetSuccessMessage("Usuario creado correctamente");
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error de negocio al crear usuario");
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear usuario");
                ModelState.AddModelError("", ex.Message);
                SetErrorMessage("Error al crear el usuario");
            }

            return View(usuario);
        }

        // GET: Usuario/Edit/5 (admin edita a otro usuario)
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Mostrando formulario de edicion para usuario ID: {Id}", id);
            var usuario = await _service.GetByIdAsync(id);
            if (usuario == null)
            {
                _logger.LogWarning("Usuario ID: {Id} no encontrado para editar", id);
                return NotFound();
            }

            // Si intenta editarse a sí mismo, redirigir a Mi Perfil
            var usuarioActual = await GetUsuarioActual();
            if (usuarioActual != null && usuarioActual.Id == id)
            {
                _logger.LogInformation("Admin intentando editar su propio usuario. Redirigiendo a Mi Perfil");
                return RedirectToAction(nameof(MiPerfil));
            }

            return View(usuario);
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                _logger.LogWarning("ID de ruta: {Id} no coincide con ID del modelo: {ModelId}", id, usuario.Id);
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState invalido al editar usuario ID: {Id}", id);
                return View(usuario);
            }

            try
            {
                _logger.LogInformation("Actualizando usuario ID: {Id} - Email: {Email}", id, usuario.Email);
                await _service.UpdateAsync(usuario);
                SetSuccessMessage("Usuario actualizado correctamente");
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error de negocio al actualizar usuario ID: {Id}", id);
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar usuario ID: {Id}", id);
                ModelState.AddModelError("", ex.Message);
                SetErrorMessage("Error al actualizar el usuario");
            }

            return View(usuario);
        }

        // GET: Usuario/Delete/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Mostrando confirmacion de eliminacion para usuario ID: {Id}", id);
            var usuario = await _service.GetByIdAsync(id);
            if (usuario == null)
            {
                _logger.LogWarning("Usuario ID: {Id} no encontrado para eliminar", id);
                return NotFound();
            }

            var usuarioActual = await GetUsuarioActual();
            if (usuarioActual != null && usuarioActual.Id == id)
            {
                _logger.LogWarning("Intento de eliminar el propio usuario");
                SetErrorMessage("No puedes eliminarte a ti mismo.");
                return RedirectToAction(nameof(Index));
            }

            return View(usuario);
        }

        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                _logger.LogInformation("Eliminando usuario ID: {Id}", id);

                var usuarioActual = await GetUsuarioActual();
                if (usuarioActual != null && usuarioActual.Id == id)
                {
                    SetErrorMessage("No puedes eliminarte a ti mismo.");
                    return RedirectToAction(nameof(Index));
                }

                await _service.DeleteAsync(id);
                SetSuccessMessage("Usuario eliminado correctamente");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error de negocio al eliminar usuario ID: {Id}", id);
                SetErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar usuario ID: {Id}", id);
                SetErrorMessage("No se puede eliminar el usuario");
            }

            return RedirectToAction(nameof(Index));
        }

        // ============================================================
        // ACCIONES DE PERFIL (cualquier usuario logueado)
        // ============================================================

        // GET: Usuario/MiPerfil
        public async Task<IActionResult> MiPerfil()
        {
            _logger.LogInformation("=== ENTRÓ A MiPerfil ===");
            _logger.LogInformation("Autenticado: {Auth}", User.Identity?.IsAuthenticated);
            _logger.LogInformation("Nombre: {Name}", User.Identity?.Name);

            var usuario = await GetUsuarioActual();
            if (usuario == null)
            {
                _logger.LogWarning("Usuario actual no encontrado");
                return NotFound();
            }
            return View(usuario);
        }

        // GET: Usuario/EditarMiPerfil
        public async Task<IActionResult> EditarMiPerfil()
        {
            _logger.LogInformation("Mostrando formulario de edicion de perfil");
            var usuario = await GetUsuarioActual();
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuario/EditarMiPerfil
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarMiPerfil(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState invalido al editar perfil");
                return View(usuario);
            }

            try
            {
                var usuarioActual = await GetUsuarioActual();
                if (usuarioActual == null || usuarioActual.Id != usuario.Id)
                {
                    _logger.LogWarning("Intento de editar un perfil ajeno");
                    return Forbid();
                }

                // Mantener el rol original (no se puede cambiar desde el perfil)
                usuario.Rol = usuarioActual.Rol;

                _logger.LogInformation("Actualizando perfil del usuario ID: {Id}", usuario.Id);
                await _service.UpdateAsync(usuario);
                SetSuccessMessage("Perfil actualizado correctamente");
                return RedirectToAction(nameof(MiPerfil));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error de negocio al actualizar perfil");
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar perfil");
                ModelState.AddModelError("", ex.Message);
                SetErrorMessage("Error al actualizar el perfil");
            }

            return View(usuario);
        }

        // GET: Usuario/CambiarPassword
        public IActionResult CambiarPassword()
        {
            return View();
        }

        // POST: Usuario/CambiarPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarPassword(string passwordActual, string passwordNueva, string confirmarPassword)
        {
            if (passwordNueva != confirmarPassword)
            {
                ModelState.AddModelError("", "Las contraseñas no coinciden");
                return View();
            }

            try
            {
                var usuario = await GetUsuarioActual();
                if (usuario == null) return NotFound();

                if (usuario.Password != passwordActual)
                {
                    ModelState.AddModelError("", "La contraseña actual es incorrecta");
                    return View();
                }

                usuario.Password = passwordNueva;
                await _service.UpdateAsync(usuario);
                SetSuccessMessage("Contraseña cambiada correctamente");
                return RedirectToAction(nameof(MiPerfil));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cambiar contraseña");
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        

        // ============================================================
        // MÉTODOS PRIVADOS
        // ============================================================

        private async Task<Usuario?> GetUsuarioActual()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                // Buscar por el email del claim en lugar del Name
                var email = User.FindFirst(ClaimTypes.Email)?.Value;
                if (!string.IsNullOrEmpty(email))
                {
                    return await _service.GetByEmailAsync(email);
                }
            }
            return null;
        }
    }
}