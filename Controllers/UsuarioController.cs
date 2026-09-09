using InmobiliariaTPI.Models;
using InmobiliariaTPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaTPI.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class UsuarioController : BaseController
    {
        private readonly IUsuarioService _service;

        public UsuarioController(IUsuarioService service, ILogger<UsuarioController> logger)
            : base(logger)
        {
            _service = service;
        }

        // GET: Usuario
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? searchTerm = null)
        {
            _logger.LogInformation("Obteniendo lista de usuarios - Pagina: {Page}, Busqueda: {SearchTerm}", page, searchTerm ?? "ninguna");
            ViewBag.SearchTerm = searchTerm;
            var usuarios = await _service.GetPagedAsync(page, pageSize, searchTerm);
            return View(usuarios);
        }

        // GET: Usuario/Details/5
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
        public IActionResult Create()
        {
            _logger.LogInformation("Mostrando formulario de creacion de usuario");
            return View();
        }

        // POST: Usuario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // GET: Usuario/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Mostrando formulario de edicion para usuario ID: {Id}", id);
            var usuario = await _service.GetByIdAsync(id);
            if (usuario == null)
            {
                _logger.LogWarning("Usuario ID: {Id} no encontrado para editar", id);
                return NotFound();
            }

            // no se puede editar a si mismo desde aqui
            var usuarioActual = await GetUsuarioActual();
            if (usuarioActual != null && usuarioActual.Id == id)
            {
                _logger.LogWarning("Intento de editar el propio usuario desde UsuarioController");
                SetErrorMessage("No puedes editarte a ti mismo. Usa la seccion de Perfil.");
                return RedirectToAction(nameof(Index));
            }

            return View(usuario);
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Mostrando confirmacion de eliminacion para usuario ID: {Id}", id);
            var usuario = await _service.GetByIdAsync(id);
            if (usuario == null)
            {
                _logger.LogWarning("Usuario ID: {Id} no encontrado para eliminar", id);
                return NotFound();
            }

            // no se puede eliminar a si mismo
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                _logger.LogInformation("Eliminando usuario ID: {Id}", id);

                // no se puede eliminar a si mismo
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

        // obtiene el usuario actual desde la sesion
        private async Task<Usuario?> GetUsuarioActual()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                var email = User.Identity.Name;
                return await _service.GetByEmailAsync(email!);
            }
            return null;
        }
    }
}