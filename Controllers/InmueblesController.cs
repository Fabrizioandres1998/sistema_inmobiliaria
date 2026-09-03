using InmobiliariaTPI.Models;
using InmobiliariaTPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace InmobiliariaTPI.Controllers
{
    public class InmueblesController : Controller
    {
        private readonly IInmuebleService _inmuebleService;
        private readonly IPropietarioService _propietarioService;
        private readonly ITipoInmuebleService _tipoInmuebleService;

        public InmueblesController(IInmuebleService inmuebleService,
                                IPropietarioService propietarioService,
                                ITipoInmuebleService tipoInmuebleService)
        {
            _inmuebleService = inmuebleService;
            _propietarioService = propietarioService;
            _tipoInmuebleService = tipoInmuebleService;
        }

        // GET: Inmueble
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? searchTerm = null)
        {
            ViewBag.SearchTerm = searchTerm;
            var inmuebles = await _inmuebleService.GetPagedAsync(page, pageSize, searchTerm);
            return View(inmuebles);
        }

        // GET: Inmueble/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var inmueble = await _inmuebleService.GetByIdAsync(id);
            if (inmueble == null)
                return NotFound();
            return View(inmueble);
        }

        // GET: Inmueble/Create
        public async Task<IActionResult> Create()
        {
            await CargarDropDowns();
            return View();
        }

        // POST: Inmueble/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Inmueble inmueble)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _inmuebleService.CreateAsync(inmueble);
                    TempData["Mensaje"] = "Inmueble creado correctamente";
                    TempData["Tipo"] = "success";
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            await CargarDropDowns();
            return View(inmueble);
        }

        // GET: Inmueble/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var inmueble = await _inmuebleService.GetByIdAsync(id);
            if (inmueble == null)
                return NotFound();
            await CargarDropDowns();
            return View(inmueble);
        }

        // POST: Inmueble/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Inmueble inmueble)
        {
            if (id != inmueble.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _inmuebleService.UpdateAsync(inmueble);
                    TempData["Mensaje"] = "Inmueble actualizado correctamente";
                    TempData["Tipo"] = "success";
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            await CargarDropDowns();
            return View(inmueble);
        }

        // GET: Inmueble/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var inmueble = await _inmuebleService.GetByIdAsync(id);
            if (inmueble == null)
                return NotFound();
            return View(inmueble);
        }

        // POST: Inmueble/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _inmuebleService.DeleteAsync(id);
                TempData["Mensaje"] = "Inmueble eliminado correctamente";
                TempData["Tipo"] = "success";
            }
            catch (Exception)
            {
                TempData["Mensaje"] = "No se puede eliminar el inmueble porque tiene reservas asociadas";
                TempData["Tipo"] = "danger";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Inmueble/Suspender/5
        public async Task<IActionResult> Suspender(int id)
        {
            await _inmuebleService.SuspenderAsync(id);
            TempData["Mensaje"] = "Inmueble suspendido correctamente";
            TempData["Tipo"] = "warning";
            return RedirectToAction(nameof(Index));
        }

        // GET: Inmueble/Activar/5
        public async Task<IActionResult> Activar(int id)
        {
            await _inmuebleService.ActivarAsync(id);
            TempData["Mensaje"] = "Inmueble activado correctamente";
            TempData["Tipo"] = "success";
            return RedirectToAction(nameof(Index));
        }

        // metodo privado para cargar los dropdowns
        private async Task CargarDropDowns()
        {
            var propietarios = await _propietarioService.GetAllAsync();
            var tipos = await _tipoInmuebleService.GetAllAsync();

            ViewBag.Propietarios = new SelectList(propietarios, "Id", "NombreCompleto");
            ViewBag.Tipos = new SelectList(tipos, "Id", "Nombre");
        }
    }
}
