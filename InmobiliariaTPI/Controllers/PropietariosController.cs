using InmobiliariaTPI.Models;
using InmobiliariaTPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaTPI.Controllers
{
    public class PropietariosController : Controller
    {
        private readonly IPropietarioService _service;

        public PropietariosController(IPropietarioService service)
        {
            _service = service;
        }

        // GET: Propietarios
        public async Task<IActionResult> Index()
        {
            var propietarios = await _service.GetAllAsync();
            return View(propietarios);
        }

        // GET: Propietarios/Details/id
        public async Task<IActionResult> Details(int id)
        {
            var propietario = await _service.GetByIdAsync(id);
            if (propietario == null)
                return NotFound();
            return View(propietario);
        }

        // GET: Propietarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Propietarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Propietario propietario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _service.CreateAsync(propietario);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(propietario);
        }

        // GET: Propietarios/Edit/id
        public async Task<IActionResult> Edit(int id)
        {
            var propietario = await _service.GetByIdAsync(id);
            if (propietario == null)
                return NotFound();
            return View(propietario);
        }

        // POST: Propietarios/Edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Propietario propietario)
        {
            if (id != propietario.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _service.UpdateAsync(propietario);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(propietario);
        }

        // GET: Propietarios/Delete/id
        public async Task<IActionResult> Delete(int id)
        {
            var propietario = await _service.GetByIdAsync(id);
            if (propietario == null)
                return NotFound();
            return View(propietario);
        }

        // POST: Propietarios/Delete/id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}