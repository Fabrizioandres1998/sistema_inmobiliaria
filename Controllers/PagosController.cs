using InmobiliariaTPI.Models;
using InmobiliariaTPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace InmobiliariaTPI.Controllers
{
    [Authorize]
    public class PagosController : BaseController
    {
        private readonly IPagoService _pagoService;
        private readonly IReservaService _reservaService;
        private readonly IUsuarioService _usuarioService;

        public PagosController(
            IPagoService pagoService,
            IReservaService reservaService,
            IUsuarioService usuarioService,
            ILogger<PagosController> logger) : base(logger)
        {
            _pagoService = pagoService;
            _reservaService = reservaService;
            _usuarioService = usuarioService;
        }

        // GET: Pagos/Index
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? searchTerm = null)
        {
            _logger.LogInformation("Obteniendo lista de pagos - Página: {Page}, Búsqueda: {SearchTerm}", page, searchTerm ?? "ninguna");
            ViewBag.SearchTerm = searchTerm;
            var pagos = await _pagoService.GetPagedAsync(page, pageSize, searchTerm);
            return View(pagos);
        }

        // GET: Pagos/Details/5
        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation("Obteniendo detalle del pago ID: {Id}", id);
            var pago = await _pagoService.GetByIdAsync(id);

            if (pago == null)
            {
                _logger.LogWarning("Pago ID: {Id} no encontrado", id);
                return NotFound();
            }

            return View(pago);
        }
        // GET: Pagos/Create/5
        public async Task<IActionResult> Create(int reservaId)
        {
            _logger.LogInformation("Mostrando formulario de creación de pago para reserva ID: {ReservaId}", reservaId);

            var reserva = await _reservaService.GetByIdAsync(reservaId);
            if (reserva == null)
                return NotFound();

            var pago = new Pago
            {
                IdReserva = reservaId,
                FechaPago = DateTime.Now.Date
            };

            return View(pago);
        }

        // POST: Pagos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pago pago)
        {
            _logger.LogInformation("=== INICIO CREATE PAGO ===");
            _logger.LogInformation("IdReserva: {IdReserva}", pago.IdReserva);
            _logger.LogInformation("Concepto: {Concepto}", pago.Concepto);
            _logger.LogInformation("Importe: {Importe}", pago.Importe);
            _logger.LogInformation("FechaPago: {FechaPago}", pago.FechaPago);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState inválido");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning("Error: {Error}", error.ErrorMessage);
                }
                TempData["Error"] = "Error al crear el pago. Verifique los datos.";
                return RedirectToAction("Details", "Reservas", new { id = pago.IdReserva });
            }

            try
            {
                var usuarioActual = await GetUsuarioActual();
                _logger.LogInformation("Usuario actual: {Usuario}", usuarioActual?.Email ?? "null");

                if (usuarioActual == null)
                {
                    _logger.LogWarning("Usuario no autenticado");
                    TempData["Error"] = "Usuario no autenticado";
                    return RedirectToAction("Details", "Reservas", new { id = pago.IdReserva });
                }

                pago.IdUsuarioCreador = usuarioActual.Id;
                pago.Estado = 1;
                pago.FechaCreacion = DateTime.Now;

                _logger.LogInformation("Intentando crear pago...");
                await _pagoService.CreateAsync(pago);
                _logger.LogInformation("Pago creado exitosamente con ID: {Id}", pago.Id);

                TempData["Mensaje"] = "Pago creado correctamente";
                TempData["Tipo"] = "success";
                return RedirectToAction("Details", "Reservas", new { id = pago.IdReserva });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear pago");
                TempData["Error"] = "Error al crear el pago: " + ex.Message;
                return RedirectToAction("Details", "Reservas", new { id = pago.IdReserva });
            }
        }

        // GET: Pagos/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Mostrando formulario de edición para pago ID: {Id}", id);
            var pago = await _pagoService.GetByIdAsync(id);
            if (pago == null)
            {
                _logger.LogWarning("Pago ID: {Id} no encontrado para editar", id);
                return NotFound();
            }

            if (pago.Estado == 0)
            {
                _logger.LogWarning("Intento de editar pago anulado ID: {Id}", id);
                SetErrorMessage("No se puede editar un pago anulado");
                return RedirectToAction("Details", "Reservas", new { id = pago.IdReserva });
            }

            return View(pago);
        }

        // POST: Pagos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Pago pago)
        {
            if (id != pago.Id)
            {
                _logger.LogWarning("ID de ruta: {Id} no coincide con ID del modelo: {ModelId}", id, pago.Id);
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState inválido al editar pago ID: {Id}", id);
                return View(pago);
            }

            try
            {
                _logger.LogInformation("Actualizando pago ID: {Id} - Concepto: {Concepto}", id, pago.Concepto);
                await _pagoService.UpdateAsync(pago);
                SetSuccessMessage("Pago actualizado correctamente");

                var pagoExistente = await _pagoService.GetByIdAsync(id);
                return RedirectToAction("Details", "Reservas", new { id = pagoExistente!.IdReserva });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error de negocio al actualizar pago ID: {Id}", id);
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar pago ID: {Id}", id);
                ModelState.AddModelError("", ex.Message);
                SetErrorMessage("Error al actualizar el pago");
            }

            return View(pago);
        }

        // GET: Pagos/Anular/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Anular(int id)
        {
            _logger.LogInformation("Mostrando confirmación de anulación para pago ID: {Id}", id);
            var pago = await _pagoService.GetByIdAsync(id);
            if (pago == null)
            {
                _logger.LogWarning("Pago ID: {Id} no encontrado para anular", id);
                return NotFound();
            }

            if (pago.Estado == 0)
            {
                _logger.LogWarning("Intento de anular pago ya anulado ID: {Id}", id);
                SetErrorMessage("El pago ya está anulado");
                return RedirectToAction("Details", "Reservas", new { id = pago.IdReserva });
            }

            return View(pago);
        }

        // POST: Pagos/Anular/5
        [HttpPost, ActionName("Anular")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AnularConfirmed(int id)
        {
            try
            {
                var pago = await _pagoService.GetByIdAsync(id);
                if (pago == null)
                    return NotFound();

                var usuarioActual = await GetUsuarioActual();

                _logger.LogInformation("Anulando pago ID: {Id}", id);
                await _pagoService.AnularAsync(id, usuarioActual!.Id);
                SetSuccessMessage("Pago anulado correctamente");
                return RedirectToAction("Details", "Reservas", new { id = pago.IdReserva });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error de negocio al anular pago ID: {Id}", id);
                SetErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al anular pago ID: {Id}", id);
                SetErrorMessage("Error al anular el pago");
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Pagos/ByReserva/5
        public async Task<IActionResult> ByReserva(int reservaId)
        {
            _logger.LogInformation("Obteniendo pagos de la reserva ID: {ReservaId}", reservaId);
            var pagos = await _pagoService.GetByReservaIdAsync(reservaId);
            var reserva = await _reservaService.GetByIdAsync(reservaId);
            ViewBag.Reserva = reserva;
            return View(pagos);
        }

        private async Task<Usuario?> GetUsuarioActual()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;
                if (!string.IsNullOrEmpty(email))
                {
                    return await _usuarioService.GetByEmailAsync(email);
                }
            }
            return null;
        }
    }
}