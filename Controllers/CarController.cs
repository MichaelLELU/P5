using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OC_p5_Express_Voitures.Models.Entities;
using OC_p5_Express_Voitures.Services.Interfaces;

namespace OC_p5_Express_Voitures.Controllers
{
    public class CarController : Controller
    {
        private readonly ICarService _carService;
        private readonly IBrandService _brandService;
        private readonly IModelService _modelService;
        private readonly IFinishingService _finishingService;

        public CarController(ICarService carService, IBrandService brandService, IModelService modelService,
            IFinishingService finishingService)
        {
            _carService = carService;
            _brandService = brandService;
            _modelService = modelService;
            _finishingService = finishingService;
        }

        // GET: Liste des voitures
        public async Task<IActionResult> IndexCarList()
        {
            var voitures = await _carService.GetAllAsync();
            return View(voitures);
        }

      
        public async Task<IActionResult> CarDetails(int? id)
        {
            if (id == null)
                return NotFound();

            var voiture = await _carService.GetByIdAsync(id.Value);
            if (voiture == null)
                return NotFound();

            return View(voiture);
        }

        // GET: Create Car
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> IndexCreateCar()
        {
            await RemplirViewBags();

            return View();
        }

        // POST: Create Car
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCar(Car voiture, IFormFile? imageFile)
        {
            // Nettoyer les "__new__"
            if (Request.Form["IdBrand"] == "__new__") voiture.IdBrand = null;
            if (Request.Form["IdModel"] == "__new__") voiture.IdModel = null;
            if (Request.Form["IdFinishing"] == "__new__") voiture.IdFinishing = null;

            string newBrandName = Request.Form["NewBrandName"];
            string newModelName = Request.Form["NewModelName"];
            string newFinishingName = Request.Form["NewFinishingName"];

            // 1. Créer si nécessaire
            if (!string.IsNullOrWhiteSpace(newBrandName))
            {
                var brand = await _brandService.CreateIfNotExistsAsync(newBrandName);
                voiture.IdBrand = brand.Id;
            }

            if (!string.IsNullOrWhiteSpace(newModelName))
            {
                var model = await _modelService.CreateIfNotExistsAsync(newModelName, voiture.IdBrand ?? 0);
                voiture.IdModel = model.Id;
            }

            if (!string.IsNullOrWhiteSpace(newFinishingName))
            {
                var finishing = await _finishingService.CreateIfNotExistsAsync(newFinishingName, voiture.IdModel ?? 0);
                voiture.IdFinishing = finishing.Id;
            }

            // 2. Effacer les erreurs précédentes liées à ces champs
            ModelState.Remove(nameof(Car.IdBrand));
            ModelState.Remove(nameof(Car.IdModel));
            ModelState.Remove(nameof(Car.IdFinishing));

            // 3. Revalider le modèle après mise à jour
            TryValidateModel(voiture);

            if (ModelState.IsValid)
            {
                // 4. Enregistrer l’image si fournie
                if (imageFile != null && imageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(imageFile.FileName);
                    var filePath = Path.Combine("wwwroot/cars/", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    voiture.ImagePath = fileName;
                }

                // 5. Enregistrer la voiture
                await _carService.CreateAsync(voiture);
                return RedirectToAction("Confirmation");
            }

            // Debug ModelState
            foreach (var entry in ModelState)
            {
                if (entry.Value.Errors.Count > 0)
                {
                    Console.WriteLine($"🛑 Champ : {entry.Key}");
                    foreach (var error in entry.Value.Errors)
                    {
                        Console.WriteLine($"    ❌ {error.ErrorMessage}");
                    }
                }
            }

            await RemplirViewBags();
            return View("IndexCreateCar", voiture);
        }





        public IActionResult Confirmation()
        {
            return View();
        }

        // GET: Edit Car
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCar(int? id)
        {
            if (id == null) return NotFound();

            var voiture = await _carService.GetByIdAsync(id.Value);
            if (voiture == null) return NotFound();

            await RemplirViewBags();
            return View(voiture);
        }

        // POST: Edit Car
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCar(int id, Car voiture)
        {
            if (id != voiture.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _carService.UpdateAsync(voiture);
                }
                catch
                {
                    if (!_carService.Exists(id)) return NotFound();
                    throw;
                }

                return RedirectToAction(nameof(IndexCarList));
            }


            await RemplirViewBags();

            return View(voiture);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCarConfirmed(int id)
        {
            await _carService.DeleteAsync(id);
            return RedirectToAction(nameof(IndexCarList));
        }

        [HttpGet]
        public async Task<IActionResult> GetModelsByBrand(int brandId)
        {
            var models = await _modelService.GetModelsByBrandIdAsync(brandId);
            var result = models.Select(m => new { m.Id, m.Name });
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetFinishingsByModel(int modelId)
        {
            var finishings = await _finishingService.GetFinishingsByModelIdAsync(modelId);
            var result = finishings.Select(f => new { f.Id, f.Name });
            return Json(result);
        }

        private async Task RemplirViewBags()
        {
            var brands = await _brandService.GetAllBrandsAsync();
            var models = await _modelService.GetAllModelsAsync();
            var finishings = await _finishingService.GetAllFinishingsAsync();

            ViewBag.Brands = new SelectList(brands, "Id", "Name");
            ViewBag.Models = new SelectList(models, "Id", "Name");
            ViewBag.Finishings = new SelectList(finishings, "Id", "Name");
        }
    }
}