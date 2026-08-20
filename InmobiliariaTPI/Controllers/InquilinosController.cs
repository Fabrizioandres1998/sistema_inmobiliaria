using InmobiliariaTPI.Models;
using InmobiliariaTPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaTPI.Controllers
{
    public class InquilinosController : Controller
    {
        private readonly IInquilinoService _service;

        public InquilinosController(IInquilinoService service)
        {
            _service = service;
        }

        // GET: Inquilinos
        public async Task<IActionResult> Index()
        {
            var inquilinos = await _service.GetAllAsync();
            return View(inquilinos);
        }

        // GET: Inquilinos/Details/id
        public async Task<IActionResult> Details(int id)
        {
            var inquilino = await _service.GetByIdAsync(id);
            if (inquilino == null)
                return NotFound();
            return View(inquilino);
        }

        // GET: Inquilinos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Inquilinos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Inquilino inquilino)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _service.CreateAsync(inquilino);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(inquilino);
        }

        // GET: Inquilinos/Edit/id
        public async Task<IActionResult> Edit(int id)
        {
            var inquilino = await _service.GetByIdAsync(id);
            if (inquilino == null)
                return NotFound();
            return View(inquilino);
        }

        // POST: Inquilinos/Edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Inquilino inquilino)
        {
            if (id != inquilino.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _service.UpdateAsync(inquilino);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(inquilino);
        }

        // GET: Inquilinos/Delete/id
        public async Task<IActionResult> Delete(int id)
        {
            var inquilino = await _service.GetByIdAsync(id);
            if (inquilino == null)
                return NotFound();
            return View(inquilino);
        }

        // POST: Inquilinos/Delete/id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
