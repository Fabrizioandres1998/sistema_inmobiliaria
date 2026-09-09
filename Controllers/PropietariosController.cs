using InmobiliariaTPI.Models;
using InmobiliariaTPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace InmobiliariaTPI.Controllers
{
    [Authorize]
    public class PropietariosController : BaseController
    {
        private readonly IPropietarioService _service;

        public PropietariosController(IPropietarioService service, ILogger<PropietariosController> logger) : base(logger)
        {
            _service = service;
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
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState inválido al crear propietario");
                return View(propietario);
            }

            try
            {
                _logger.LogInformation("Creando nuevo propietario: {Nombre} - DNI: {Dni}", propietario.NombreCompleto, propietario.Dni);
                await _service.CreateAsync(propietario);
                SetSuccessMessage($"Propietario '{propietario.NombreCompleto}' creado exitosamente");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                AddModelErrors(ex);
                return View(propietario);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Mostrando formulario de edición para propietario ID: {Id}", id);
            var propietario = await _service.GetByIdAsync(id);
            if (propietario == null)
            {
                _logger.LogWarning("Propietario ID: {Id} no encontrado para editar", id);
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
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState inválido al editar propietario ID: {Id}", id);
                return View(propietario);
            }

            try
            {
                _logger.LogInformation("Actualizando propietario ID: {Id} - Nombre: {Nombre}", id, propietario.NombreCompleto);
                await _service.UpdateAsync(propietario);
                SetSuccessMessage($"Propietario '{propietario.NombreCompleto}' actualizado exitosamente");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                AddModelErrors(ex);
                return View(propietario);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Mostrando confirmación de eliminación para propietario ID: {Id}", id);
            var propietario = await _service.GetByIdAsync(id);
            if (propietario == null)
            {
                _logger.LogWarning("Propietario ID: {Id} no encontrado para eliminar", id);
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
                SetSuccessMessage("Propietario eliminado exitosamente");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                AddModelErrors(ex);
                var propietario = await _service.GetByIdAsync(id);
                return View(propietario);
            }
        }
    }
}