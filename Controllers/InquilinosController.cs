using InmobiliariaTPI.Models;
using InmobiliariaTPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InmobiliariaTPI.Controllers
{
    public class InquilinosController : Controller
    {
        private readonly IInquilinoService _service;
        private readonly ILogger<InquilinosController> _logger;

        public InquilinosController(IInquilinoService service, ILogger<InquilinosController> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Obteniendo lista de inquilinos");
            var inquilinos = await _service.GetAllAsync();
            return View(inquilinos);
        }

        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation("Obteniendo detalle del inquilino ID: {Id}", id);
            var inquilino = await _service.GetByIdAsync(id);
            if (inquilino == null)
            {
                _logger.LogWarning("Inquilino ID: {Id} no encontrado", id);
                TempData["Error"] = "Inquilino no encontrado";
                return NotFound();
            }
            return View(inquilino);
        }

        public IActionResult Create()
        {
            _logger.LogInformation("Mostrando formulario de creación de inquilino");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Inquilino inquilino)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInformation("Creando nuevo inquilino: {Nombre} - DNI: {Dni}", inquilino.NombreCompleto, inquilino.Dni);
                    await _service.CreateAsync(inquilino);
                    _logger.LogInformation("Inquilino creado exitosamente. ID: {Id}", inquilino.Id);
                    TempData["Mensaje"] = $"Inquilino '{inquilino.NombreCompleto}' creado exitosamente";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al crear inquilino: {Nombre} - DNI: {Dni}", inquilino.NombreCompleto, inquilino.Dni);
                    TempData["Error"] = ex.Message;
                    ModelState.AddModelError("", ex.Message);
                }
            }
            else
            {
                _logger.LogWarning("ModelState inválido al crear inquilino");
                TempData["Error"] = "Por favor complete todos los campos requeridos";
            }
            return View(inquilino);
        }

        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Mostrando formulario de edición para inquilino ID: {Id}", id);
            var inquilino = await _service.GetByIdAsync(id);
            if (inquilino == null)
            {
                _logger.LogWarning("Inquilino ID: {Id} no encontrado para editar", id);
                TempData["Error"] = "Inquilino no encontrado";
                return NotFound();
            }
            return View(inquilino);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Inquilino inquilino)
        {
            if (id != inquilino.Id)
            {
                _logger.LogWarning("ID de ruta: {Id} no coincide con ID del modelo: {ModelId}", id, inquilino.Id);
                TempData["Error"] = "El ID no coincide";
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInformation("Actualizando inquilino ID: {Id} - Nombre: {Nombre}", id, inquilino.NombreCompleto);
                    await _service.UpdateAsync(inquilino);
                    _logger.LogInformation("Inquilino ID: {Id} actualizado exitosamente", id);
                    TempData["Mensaje"] = $"Inquilino '{inquilino.NombreCompleto}' actualizado exitosamente";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al actualizar inquilino ID: {Id}", id);
                    TempData["Error"] = ex.Message;
                    ModelState.AddModelError("", ex.Message);
                }
            }
            else
            {
                _logger.LogWarning("ModelState inválido al editar inquilino ID: {Id}", id);
                TempData["Error"] = "Por favor complete todos los campos requeridos";
            }
            return View(inquilino);
        }

        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Mostrando confirmación de eliminación para inquilino ID: {Id}", id);
            var inquilino = await _service.GetByIdAsync(id);
            if (inquilino == null)
            {
                _logger.LogWarning("Inquilino ID: {Id} no encontrado para eliminar", id);
                TempData["Error"] = "Inquilino no encontrado";
                return NotFound();
            }
            return View(inquilino);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                _logger.LogInformation("Eliminando inquilino ID: {Id}", id);
                await _service.DeleteAsync(id);
                _logger.LogInformation("Inquilino ID: {Id} eliminado exitosamente", id);
                TempData["Mensaje"] = "Inquilino eliminado exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar inquilino ID: {Id}", id);
                TempData["Error"] = ex.Message;
                ModelState.AddModelError("", ex.Message);
                var inquilino = await _service.GetByIdAsync(id);
                return View(inquilino);
            }
        }
    }
}