using InmobiliariaTPI.Models;
using InmobiliariaTPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using InmobiliariaTPI.ViewModels;

namespace InmobiliariaTPI.Controllers
{
    public class ReservasController : Controller
    {
        private readonly IReservaService _reservaService;
        private readonly IInmuebleService _inmuebleService;
        private readonly IInquilinoService _inquilinoService;
        // private readonly IUsuarioService _usuarioService;

        public ReservasController(
            IReservaService reservaService,
            IInmuebleService inmuebleService,
            IInquilinoService inquilinoService
            // IUsuarioService usuarioService
            )
        {
            _reservaService = reservaService;
            _inmuebleService = inmuebleService;
            _inquilinoService = inquilinoService;
            // _usuarioService = usuarioService;
        }

        // GET: Reserva
        public async Task<IActionResult> Index()
        {
            var reservas = await _reservaService.GetAllAsync();
            return View(reservas);
        }

        // GET: Reserva/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var reserva = await _reservaService.GetByIdAsync(id);
            if (reserva == null)
                return NotFound();
            return View(reserva);
        }

        // GET: Reserva/Create
        public async Task<IActionResult> Create()
        {
            await CargarDropDowns();
            return View();
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
                    TempData["Mensaje"] = "Reserva creada correctamente";
                    TempData["Tipo"] = "success";
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            await CargarDropDowns();
            return View(reserva);
        }

        // GET: Reserva/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var reserva = await _reservaService.GetByIdAsync(id);
            if (reserva == null)
                return NotFound();
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

            if (ModelState.IsValid)
            {
                try
                {
                    await _reservaService.UpdateAsync(reserva);
                    TempData["Mensaje"] = "Reserva actualizada correctamente";
                    TempData["Tipo"] = "success";
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            await CargarDropDowns();
            return View(reserva);
        }

        // GET: Reserva/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var reserva = await _reservaService.GetByIdAsync(id);
            if (reserva == null)
                return NotFound();
            return View(reserva);
        }

        // POST: Reserva/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _reservaService.DeleteAsync(id);
                TempData["Mensaje"] = "Reserva eliminada correctamente";
                TempData["Tipo"] = "success";
            }
            catch (Exception)
            {
                TempData["Mensaje"] = "No se puede eliminar la reserva";
                TempData["Tipo"] = "danger";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Reserva/Finalizar/5
        public async Task<IActionResult> Finalizar(int id)
        {
            var reserva = await _reservaService.GetByIdAsync(id);
            if (reserva == null)
                return NotFound();

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
            if (id != viewModel.Id)
                return NotFound();

            try
            {
                await _reservaService.FinalizarAsync(id, viewModel.FechaTerminacion, 1); // despues viene de sesion
                TempData["Mensaje"] = "Reserva finalizada correctamente";
                TempData["Tipo"] = "success";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                var reserva = await _reservaService.GetByIdAsync(id);
                viewModel.MultaCalculada = CalcularMulta(reserva!);
                return View(viewModel);
            }
        }

        // GET: Reserva/Renovar/5
        public async Task<IActionResult> Renovar(int id)
        {
            var reserva = await _reservaService.GetByIdAsync(id);
            if (reserva == null)
                return NotFound();

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
                    TempData["Mensaje"] = "Reserva renovada correctamente";
                    TempData["Tipo"] = "success";
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            await CargarDropDowns();
            return View(nuevaReserva);
        }

        // GET: Reserva/Vigentes
        public async Task<IActionResult> Vigentes()
        {
            var reservas = await _reservaService.GetVigentesAsync();
            return View(reservas);
        }

        // carga los dropdowns de inmuebles e inquilinos
        private async Task CargarDropDowns()
        {
            var inmuebles = await _inmuebleService.GetAllAsync();
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