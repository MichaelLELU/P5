using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OC_p5_Express_Voitures.Models.Entities;
using OC_p5_Express_Voitures.Services.Interfaces;

namespace OC_p5_Express_Voitures.Controllers
{
    public class ReparationController : Controller
    {
        private readonly IReparationService _reparationService;

        public ReparationController(IReparationService reparationService)
        {
            _reparationService = reparationService;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _reparationService.GetAllAsync();
            return View(data);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reparation reparation)
        {
            if (ModelState.IsValid)
            {
                await _reparationService.CreateAsync(reparation);
                return RedirectToAction(nameof(Index));
            }
            return View(reparation);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var r = await _reparationService.GetByIdAsync(id);
            if (r == null) return NotFound();
            return View(r);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Reparation reparation)
        {
            if (id != reparation.Id) return NotFound();

            if (ModelState.IsValid)
            {
                await _reparationService.UpdateAsync(reparation);
                return RedirectToAction(nameof(Index));
            }

            return View(reparation);
        }

    }
}
