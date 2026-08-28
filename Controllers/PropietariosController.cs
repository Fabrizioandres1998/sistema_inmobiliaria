using InmobiliariaTPI.Models;
using InmobiliariaTPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InmobiliariaTPI.Controllers
{
    public class PropietariosController : Controller
    {
        private readonly IPropietarioService _service;
        private readonly ILogger<PropietariosController> _logger;

        public PropietariosController(IPropietarioService service, ILogger<PropietariosController> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Obteniendo lista de propietarios");
            var propietarios = await _service.GetAllAsync();
            return View(propietarios);
        }

        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation("Obteniendo detalle del propietario ID: {Id}", id);
            var propietario = await _service.GetByIdAsync(id);
            if (propietario == null)
            {
                _logger.LogWarning("Propietario ID: {Id} no encontrado", id);
                TempData["Error"] = "Propietario no encontrado";
                return NotFound();
            }
            return View(propietario);
        }

        public IActionResult Create()
        {
            _logger.LogInformation("Mostrando formulario de creación de propietario");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Propietario propietario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInformation("Creando nuevo propietario: {Nombre} - DNI: {Dni}", propietario.NombreCompleto, propietario.Dni);
                    await _service.CreateAsync(propietario);
                    _logger.LogInformation("Propietario creado exitosamente. ID: {Id}", propietario.Id);
                    TempData["Mensaje"] = $"Propietario '{propietario.NombreCompleto}' creado exitosamente";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al crear propietario: {Nombre} - DNI: {Dni}", propietario.NombreCompleto, propietario.Dni);
                    TempData["Error"] = ex.Message;
                    ModelState.AddModelError("", ex.Message);
                }
            }
            else
            {
                _logger.LogWarning("ModelState inválido al crear propietario");
                TempData["Error"] = "Por favor complete todos los campos requeridos";
            }
            return View(propietario);
        }

        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Mostrando formulario de edición para propietario ID: {Id}", id);
            var propietario = await _service.GetByIdAsync(id);
            if (propietario == null)
            {
                _logger.LogWarning("Propietario ID: {Id} no encontrado para editar", id);
                TempData["Error"] = "Propietario no encontrado";
                return NotFound();
            }
            return View(propietario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Propietario propietario)
        {
            if (id != propietario.Id)
            {
                _logger.LogWarning("ID de ruta: {Id} no coincide con ID del modelo: {ModelId}", id, propietario.Id);
                TempData["Error"] = "El ID no coincide";
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInformation("Actualizando propietario ID: {Id} - Nombre: {Nombre}", id, propietario.NombreCompleto);
                    await _service.UpdateAsync(propietario);
                    _logger.LogInformation("Propietario ID: {Id} actualizado exitosamente", id);
                    TempData["Mensaje"] = $"Propietario '{propietario.NombreCompleto}' actualizado exitosamente";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al actualizar propietario ID: {Id}", id);
                    TempData["Error"] = ex.Message;
                    ModelState.AddModelError("", ex.Message);
                }
            }
            else
            {
                _logger.LogWarning("ModelState inválido al editar propietario ID: {Id}", id);
                TempData["Error"] = "Por favor complete todos los campos requeridos";
            }
            return View(propietario);
        }

        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Mostrando confirmación de eliminación para propietario ID: {Id}", id);
            var propietario = await _service.GetByIdAsync(id);
            if (propietario == null)
            {
                _logger.LogWarning("Propietario ID: {Id} no encontrado para eliminar", id);
                TempData["Error"] = "Propietario no encontrado";
                return NotFound();
            }
            return View(propietario);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                _logger.LogInformation("Eliminando propietario ID: {Id}", id);
                await _service.DeleteAsync(id);
                _logger.LogInformation("Propietario ID: {Id} eliminado exitosamente", id);
                TempData["Mensaje"] = "Propietario eliminado exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar propietario ID: {Id}", id);
                TempData["Error"] = ex.Message;
                ModelState.AddModelError("", ex.Message);
                var propietario = await _service.GetByIdAsync(id);
                return View(propietario);
            }
        }
    }
}