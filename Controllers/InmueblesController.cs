using InmobiliariaTPI.Models;
using InmobiliariaTPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InmobiliariaTPI.Controllers
{
    public class InmueblesController : BaseController
    {
        private readonly IInmuebleService _inmuebleService;
        private readonly IPropietarioService _propietarioService;
        private readonly ITipoInmuebleService _tipoInmuebleService;

        public InmueblesController(
            IInmuebleService inmuebleService,
            IPropietarioService propietarioService,
            ITipoInmuebleService tipoInmuebleService,
            ILogger<InmueblesController> logger) : base(logger)
        {
            _inmuebleService = inmuebleService;
            _propietarioService = propietarioService;
            _tipoInmuebleService = tipoInmuebleService;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? searchTerm = null)
        {
            _logger.LogInformation("Obteniendo lista de inmuebles - Página: {Page}, Búsqueda: {SearchTerm}", page, searchTerm ?? "ninguna");
            ViewBag.SearchTerm = searchTerm;
            var inmuebles = await _inmuebleService.GetPagedAsync(page, pageSize, searchTerm);
            return View(inmuebles);
        }

        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation("Obteniendo detalle del inmueble ID: {Id}", id);
            var inmueble = await _inmuebleService.GetByIdAsync(id);
            if (inmueble == null)
            {
                _logger.LogWarning("Inmueble ID: {Id} no encontrado", id);
                return NotFound();
            }
            return View(inmueble);
        }

        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("Mostrando formulario de creación de inmueble");
            await CargarDropDowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Inmueble inmueble)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState inválido al crear inmueble");
                await CargarDropDowns();
                return View(inmueble);
            }

            try
            {
                _logger.LogInformation("Creando nuevo inmueble - Dirección: {Direccion}", inmueble.Direccion);
                await _inmuebleService.CreateAsync(inmueble);
                SetSuccessMessage("Inmueble creado correctamente");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                AddModelErrors(ex);
                await CargarDropDowns();
                return View(inmueble);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Mostrando formulario de edición para inmueble ID: {Id}", id);
            var inmueble = await _inmuebleService.GetByIdAsync(id);
            if (inmueble == null)
            {
                _logger.LogWarning("Inmueble ID: {Id} no encontrado para editar", id);
                return NotFound();
            }
            await CargarDropDowns();
            return View(inmueble);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Inmueble inmueble)
        {
            if (id != inmueble.Id)
            {
                _logger.LogWarning("ID de ruta: {Id} no coincide con ID del modelo: {ModelId}", id, inmueble.Id);
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState inválido al editar inmueble ID: {Id}", id);
                await CargarDropDowns();
                return View(inmueble);
            }

            try
            {
                _logger.LogInformation("Actualizando inmueble ID: {Id} - Dirección: {Direccion}", id, inmueble.Direccion);
                await _inmuebleService.UpdateAsync(inmueble);
                SetSuccessMessage("Inmueble actualizado correctamente");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                AddModelErrors(ex);
                await CargarDropDowns();
                return View(inmueble);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Mostrando confirmación de eliminación para inmueble ID: {Id}", id);
            var inmueble = await _inmuebleService.GetByIdAsync(id);
            if (inmueble == null)
            {
                _logger.LogWarning("Inmueble ID: {Id} no encontrado para eliminar", id);
                return NotFound();
            }
            return View(inmueble);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                _logger.LogInformation("Eliminando inmueble ID: {Id}", id);
                await _inmuebleService.DeleteAsync(id);
                SetSuccessMessage("Inmueble eliminado correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar inmueble ID: {Id}", id);
                SetErrorMessage("No se puede eliminar el inmueble porque tiene reservas asociadas");
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Suspender(int id)
        {
            try
            {
                _logger.LogInformation("Suspendiendo inmueble ID: {Id}", id);
                await _inmuebleService.SuspenderAsync(id);
                SetWarningMessage("Inmueble suspendido correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al suspender inmueble ID: {Id}", id);
                SetErrorMessage("No se puede suspender el inmueble");
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Activar(int id)
        {
            try
            {
                _logger.LogInformation("Activando inmueble ID: {Id}", id);
                await _inmuebleService.ActivarAsync(id);
                SetSuccessMessage("Inmueble activado correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al activar inmueble ID: {Id}", id);
                SetErrorMessage("No se puede activar el inmueble");
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task CargarDropDowns()
        {
            var propietarios = await _propietarioService.GetAllAsync();
            var tipos = await _tipoInmuebleService.GetAllAsync();

            ViewBag.Propietarios = new SelectList(propietarios, "Id", "NombreCompleto");
            ViewBag.Tipos = new SelectList(tipos, "Id", "Nombre");
        }
    }
}