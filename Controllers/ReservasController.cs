using InmobiliariaTPI.Models;
using InmobiliariaTPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using InmobiliariaTPI.ViewModels;

namespace InmobiliariaTPI.Controllers
{
    [Authorize]
    public class ReservasController : BaseController
    {
        private readonly IReservaService _reservaService;
        private readonly IInmuebleService _inmuebleService;
        private readonly IInquilinoService _inquilinoService;
        // private readonly IUsuarioService _usuarioService;

        public ReservasController(
            IReservaService reservaService,
            IInmuebleService inmuebleService,
            IInquilinoService inquilinoService,
            ILogger<ReservasController> logger
            // IUsuarioService usuarioService
            ) : base(logger)
        {
            _reservaService = reservaService;
            _inmuebleService = inmuebleService;
            _inquilinoService = inquilinoService;
            // _usuarioService = usuarioService;
        }

        // GET: Reserva
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Obteniendo lista de reservas");
            var reservas = await _reservaService.GetAllAsync();
            return View(reservas);
        }

        // GET: Reserva/Details/5
        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation("Obteniendo detalle de reserva ID: {Id}", id);
            var reserva = await _reservaService.GetByIdAsync(id);
            if (reserva == null)
            {
                _logger.LogWarning("Reserva ID: {Id} no encontrada", id);
                return NotFound();
            }
            return View(reserva);
        }

        // GET: Reserva/Create
        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("Mostrando formulario de creación de reserva");
            await CargarDropDowns();

            var reserva = new Reserva
            {
                FechaInicio = DateTime.Now.Date,
                FechaFin = DateTime.Now.Date.AddDays(7),
                MontoPorDia = 0
            };

            return View(reserva);
        }

        // POST: Reserva/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reserva reserva)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    reserva.IdUsuarioCreador = 1; // por ahora hardcodeado, despues viene de sesion
                    await _reservaService.CreateAsync(reserva);
                    SetSuccessMessage("Reserva creada correctamente");
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogWarning(ex, "Error de negocio al crear reserva");
                    ModelState.AddModelError("", ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al crear reserva");
                    ModelState.AddModelError("", ex.Message);
                    SetErrorMessage("Error al crear la reserva");
                }
            }
            await CargarDropDowns();
            return View(reserva);
        }

        // GET: Reserva/GetPrecioPorDia
        [HttpGet]
        public async Task<IActionResult> GetPrecioPorDia(int inmuebleId)
        {
            var inmueble = await _inmuebleService.GetByIdAsync(inmuebleId);
            if (inmueble == null)
                return Json(new { success = false, message = "Inmueble no encontrado" });

            return Json(new { success = true, precio = inmueble.PrecioPorDia });
        }

        // GET: Reserva/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Mostrando formulario de edicion para reserva ID: {Id}", id);
            var reserva = await _reservaService.GetByIdAsync(id);
            if (reserva == null)
            {
                _logger.LogWarning("Reserva ID: {Id} no encontrada para editar", id);
                return NotFound();
            }

            // Validar que la reserva esté activa
            if (!reserva.Estado.Equals("Activa", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning("Intento de editar reserva no activa ID: {Id}", id);
                SetErrorMessage("No se puede editar una reserva que no está activa");
                return RedirectToAction(nameof(Index));
            }

            await CargarDropDowns();
            return View(reserva);
        }
        // POST: Reserva/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Reserva reserva)
        {
            if (id != reserva.Id)
                return NotFound();

            // Validar que la reserva esté activa
            var existente = await _reservaService.GetByIdAsync(id);
            if (existente == null)
                return NotFound();

            if (!existente.Estado.Equals("Activa", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning("Intento de editar reserva no activa ID: {Id}", id);
                SetErrorMessage("No se puede editar una reserva que no está activa");
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState inválido al editar reserva ID: {Id}", id);
                await CargarDropDowns();
                return View(reserva);
            }

            try
            {
                _logger.LogInformation("Actualizando reserva ID: {Id}", id);
                await _reservaService.UpdateAsync(reserva);
                SetSuccessMessage("Reserva actualizada correctamente");
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error de negocio al actualizar reserva ID: {Id}", id);
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar reserva ID: {Id}", id);
                ModelState.AddModelError("", ex.Message);
                SetErrorMessage("Error al actualizar la reserva");
            }

            await CargarDropDowns();
            return View(reserva);
        }

        // GET: Reserva/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Mostrando confirmación de eliminación para reserva ID: {Id}", id);
            var reserva = await _reservaService.GetByIdAsync(id);
            if (reserva == null)
            {
                _logger.LogWarning("Reserva ID: {Id} no encontrada para eliminar", id);
                return NotFound();
            }
            return View(reserva);
        }

        // POST: Reserva/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                _logger.LogInformation("Eliminando reserva ID: {Id}", id);
                await _reservaService.DeleteAsync(id);
                SetSuccessMessage("Reserva eliminada correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar reserva ID: {Id}", id);
                SetErrorMessage("No se puede eliminar la reserva");
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Reserva/Finalizar/5
        public async Task<IActionResult> Finalizar(int id)
        {
            _logger.LogInformation("Mostrando formulario de finalización para reserva ID: {Id}", id);
            var reserva = await _reservaService.GetByIdAsync(id);
            if (reserva == null)
            {
                _logger.LogWarning("Reserva ID: {Id} no encontrada para finalizar", id);
                return NotFound();
            }

            var viewModel = new FinalizarReservaViewModel
            {
                Id = reserva.Id,
                FechaTerminacion = DateTime.Now,
                MultaCalculada = CalcularMulta(reserva)
            };

            return View(viewModel);
        }

        // POST: Reserva/Finalizar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Finalizar(int id, FinalizarReservaViewModel viewModel)
        {
            _logger.LogInformation("🚀 ENTRO AL POST DE FINALIZAR - ID: {Id}, Fecha: {Fecha}", id, viewModel.FechaTerminacion);

            if (id != viewModel.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("⚠️ ModelState inválido en Finalizar");
                var reserva = await _reservaService.GetByIdAsync(id);
                if (reserva != null)
                {
                    viewModel.MultaCalculada = CalcularMulta(reserva);
                }
                return View(viewModel);
            }

            try
            {
                _logger.LogInformation("✅ Intentando finalizar reserva ID: {Id}", id);
                await _reservaService.FinalizarAsync(id, viewModel.FechaTerminacion, 1);
                SetSuccessMessage("Reserva finalizada correctamente");
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error de negocio al finalizar reserva ID: {Id}", id);
                ModelState.AddModelError("", ex.Message);
                var reserva = await _reservaService.GetByIdAsync(id);
                if (reserva != null)
                {
                    viewModel.MultaCalculada = CalcularMulta(reserva);
                }
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al finalizar reserva ID: {Id}", id);
                ModelState.AddModelError("", ex.Message);
                SetErrorMessage("Error al finalizar la reserva");
                return View(viewModel);
            }
        }

        // GET: Reserva/Renovar/5
        public async Task<IActionResult> Renovar(int id)
        {
            _logger.LogInformation("Mostrando formulario de renovación para reserva ID: {Id}", id);
            var reserva = await _reservaService.GetByIdAsync(id);
            if (reserva == null)
            {
                _logger.LogWarning("Reserva ID: {Id} no encontrada para renovar", id);
                return NotFound();
            }

            var nuevaReserva = new Reserva
            {
                IdInmueble = reserva.IdInmueble,
                IdInquilino = reserva.IdInquilino,
                FechaInicio = reserva.FechaFin.AddDays(1),
                FechaFin = reserva.FechaFin.AddDays(7),
                MontoPorDia = reserva.MontoPorDia,
                IdUsuarioCreador = 1 // despues viene de sesion
            };

            await CargarDropDowns();
            return View(nuevaReserva);
        }

        // POST: Reserva/Renovar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Renovar(int id, Reserva nuevaReserva)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    nuevaReserva.IdUsuarioCreador = 1; // despues viene de sesion
                    await _reservaService.RenovarAsync(nuevaReserva);
                    SetSuccessMessage("Reserva renovada correctamente");
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogWarning(ex, "Error de negocio al renovar reserva ID: {Id}", id);
                    ModelState.AddModelError("", ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al renovar reserva ID: {Id}", id);
                    ModelState.AddModelError("", ex.Message);
                    SetErrorMessage("Error al renovar la reserva");
                }
            }
            await CargarDropDowns();
            return View(nuevaReserva);
        }

        // GET: Reserva/Vigentes
        public async Task<IActionResult> Vigentes()
        {
            _logger.LogInformation("Obteniendo reservas vigentes");
            var reservas = await _reservaService.GetVigentesAsync();
            return View(reservas);
        }

        // carga los dropdowns de inmuebles e inquilinos
        // carga los dropdowns de inmuebles e inquilinos
        private async Task CargarDropDowns()
        {
            var inmuebles = await _inmuebleService.GetDisponiblesAsync();
            var inquilinos = await _inquilinoService.GetAllAsync();

            ViewBag.Inmuebles = new SelectList(inmuebles, "Id", "Direccion");
            ViewBag.Inquilinos = new SelectList(inquilinos, "Id", "NombreCompleto");
        }

        // calcula la multa segun los dias transcurridos
        private decimal CalcularMulta(Reserva reserva)
        {
            var diasOriginales = (reserva.FechaFin - reserva.FechaInicio).Days;
            var diasTranscurridos = (DateTime.Now - reserva.FechaInicio).Days;
            var porcentaje = 0m;

            if (diasTranscurridos < diasOriginales / 2)
                porcentaje = 0.5m;  // 50% si paso menos de la mitad
            else
                porcentaje = 0.25m; // 25% si paso mas de la mitad

            return reserva.MontoPorDia * diasOriginales * porcentaje;
        }
    }
}