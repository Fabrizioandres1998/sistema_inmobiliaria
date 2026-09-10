using InmobiliariaTPI.Models;
using InmobiliariaTPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace InmobiliariaTPI.Controllers
{
    [Authorize]
    public class InquilinosController : BaseController
    {
        private readonly IInquilinoService _service;

        public InquilinosController(IInquilinoService service, ILogger<InquilinosController> logger) : base(logger)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? searchTerm = null)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            _logger.LogInformation("Obteniendo lista de inquilinos - Pagina: {Page}, Busqueda: {SearchTerm}", page, searchTerm ?? "ninguna");
            ViewBag.SearchTerm = searchTerm;
            var inquilinos = await _service.GetPagedAsync(page, pageSize, searchTerm);
            return View(inquilinos);
        }

        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation("Obteniendo detalle del inquilino ID: {Id}", id);
            var inquilino = await _service.GetByIdAsync(id);
            if (inquilino == null)
            {
                _logger.LogWarning("Inquilino ID: {Id} no encontrado", id);
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
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState inválido al crear inquilino");
                return View(inquilino);
            }

            try
            {
                _logger.LogInformation("Creando nuevo inquilino: {Nombre} - DNI: {Dni}", inquilino.NombreCompleto, inquilino.Dni);
                await _service.CreateAsync(inquilino);
                SetSuccessMessage($"Inquilino '{inquilino.NombreCompleto}' creado exitosamente");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                AddModelErrors(ex);
                return View(inquilino);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Mostrando formulario de edición para inquilino ID: {Id}", id);
            var inquilino = await _service.GetByIdAsync(id);
            if (inquilino == null)
            {
                _logger.LogWarning("Inquilino ID: {Id} no encontrado para editar", id);
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
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState inválido al editar inquilino ID: {Id}", id);
                return View(inquilino);
            }

            try
            {
                _logger.LogInformation("Actualizando inquilino ID: {Id} - Nombre: {Nombre}", id, inquilino.NombreCompleto);
                await _service.UpdateAsync(inquilino);
                SetSuccessMessage($"Inquilino '{inquilino.NombreCompleto}' actualizado exitosamente");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                AddModelErrors(ex);
                return View(inquilino);
            }
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Mostrando confirmación de eliminación para inquilino ID: {Id}", id);
            var inquilino = await _service.GetByIdAsync(id);
            if (inquilino == null)
            {
                _logger.LogWarning("Inquilino ID: {Id} no encontrado para eliminar", id);
                return NotFound();
            }
            return View(inquilino);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                _logger.LogInformation("Eliminando inquilino ID: {Id}", id);
                await _service.DeleteAsync(id);
                SetSuccessMessage("Inquilino eliminado exitosamente");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                AddModelErrors(ex);
                var inquilino = await _service.GetByIdAsync(id);
                return View(inquilino);
            }
        }
    }
}