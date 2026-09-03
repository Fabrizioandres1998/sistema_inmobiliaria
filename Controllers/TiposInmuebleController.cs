using InmobiliariaTPI.Models;
using InmobiliariaTPI.Services;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace InmobiliariaTPI.Controllers
{
    public class TipoInmuebleController : Controller
    {
        private readonly ITipoInmuebleService _service;

        public TipoInmuebleController(ITipoInmuebleService service)
        {
            _service = service;
        }

        // GET: TipoInmueble
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? searchTerm = null)
        {
            ViewBag.SearchTerm = searchTerm;
            var tipos = await _service.GetPagedAsync(page, pageSize, searchTerm);
            return View(tipos);
        }

        // GET: TipoInmueble/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var tipo = await _service.GetByIdAsync(id);
            if (tipo == null)
                return NotFound();
            return View(tipo);
        }

        // GET: TipoInmueble/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoInmueble/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TipoInmueble tipo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _service.CreateAsync(tipo);
                    TempData["Mensaje"] = "Tipo de inmueble creado correctamente";
                    TempData["Tipo"] = "success";
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(tipo);
        }

        // GET: TipoInmueble/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var tipo = await _service.GetByIdAsync(id);
            if (tipo == null)
                return NotFound();
            return View(tipo);
        }

        // POST: TipoInmueble/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TipoInmueble tipo)
        {
            if (id != tipo.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _service.UpdateAsync(tipo);
                    TempData["Mensaje"] = "Tipo de inmueble actualizado correctamente";
                    TempData["Tipo"] = "success";
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(tipo);
        }

        // GET: TipoInmueble/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var tipo = await _service.GetByIdAsync(id);
            if (tipo == null)
                return NotFound();
            return View(tipo);
        }

        // POST: TipoInmueble/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                TempData["Mensaje"] = "Tipo de inmueble eliminado correctamente";
                TempData["Tipo"] = "success";
            }
            catch (Exception)
            {
                TempData["Mensaje"] = "No se puede eliminar el tipo porque tiene inmuebles asociados";
                TempData["Tipo"] = "danger";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
