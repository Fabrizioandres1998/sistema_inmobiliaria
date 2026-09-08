using InmobiliariaTPI.Models;
using InmobiliariaTPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaTPI.Controllers
{
    public class TipoInmuebleController : BaseController
    {
        private readonly ITipoInmuebleService _service;

        public TipoInmuebleController(ITipoInmuebleService service, ILogger<TipoInmuebleController> logger) : base(logger)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? searchTerm = null)
        {
            _logger.LogInformation("Obteniendo lista de tipos de inmueble - Página: {Page}, Búsqueda: {SearchTerm}", page, searchTerm ?? "ninguna");
            ViewBag.SearchTerm = searchTerm;
            var tipos = await _service.GetPagedAsync(page, pageSize, searchTerm);
            return View(tipos);
        }

        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation("Obteniendo detalle del tipo de inmueble ID: {Id}", id);
            var tipo = await _service.GetByIdAsync(id);
            if (tipo == null)
            {
                _logger.LogWarning("Tipo de inmueble ID: {Id} no encontrado", id);
                return NotFound();
            }
            return View(tipo);
        }

        public IActionResult Create()
        {
            _logger.LogInformation("Mostrando formulario de creación de tipo de inmueble");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TipoInmueble tipo)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState inválido al crear tipo de inmueble");
                return View(tipo);
            }

            try
            {
                _logger.LogInformation("Creando nuevo tipo de inmueble: {Nombre}", tipo.Nombre);
                await _service.CreateAsync(tipo);
                SetSuccessMessage("Tipo de inmueble creado correctamente");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                AddModelErrors(ex);
                return View(tipo);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Mostrando formulario de edición para tipo de inmueble ID: {Id}", id);
            var tipo = await _service.GetByIdAsync(id);
            if (tipo == null)
            {
                _logger.LogWarning("Tipo de inmueble ID: {Id} no encontrado para editar", id);
                return NotFound();
            }
            return View(tipo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TipoInmueble tipo)
        {
            if (id != tipo.Id)
            {
                _logger.LogWarning("ID de ruta: {Id} no coincide con ID del modelo: {ModelId}", id, tipo.Id);
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState inválido al editar tipo de inmueble ID: {Id}", id);
                return View(tipo);
            }

            try
            {
                _logger.LogInformation("Actualizando tipo de inmueble ID: {Id} - Nombre: {Nombre}", id, tipo.Nombre);
                await _service.UpdateAsync(tipo);
                SetSuccessMessage("Tipo de inmueble actualizado correctamente");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                AddModelErrors(ex);
                return View(tipo);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Mostrando confirmación de eliminación para tipo de inmueble ID: {Id}", id);
            var tipo = await _service.GetByIdAsync(id);
            if (tipo == null)
            {
                _logger.LogWarning("Tipo de inmueble ID: {Id} no encontrado para eliminar", id);
                return NotFound();
            }
            return View(tipo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                _logger.LogInformation("Eliminando tipo de inmueble ID: {Id}", id);
                await _service.DeleteAsync(id);
                SetSuccessMessage("Tipo de inmueble eliminado correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar tipo de inmueble ID: {Id}", id);
                SetErrorMessage("No se puede eliminar el tipo porque tiene inmuebles asociados");
            }
            return RedirectToAction(nameof(Index));
        }
    }
}