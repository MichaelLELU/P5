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

        public IActionResult DeletedConfirmation()
        {
            if (TempData["DeletedCarYear"] == null ||
                TempData["DeletedCarBrand"] == null ||
                TempData["DeletedCarModel"] == null)
            {
                return RedirectToAction(nameof(IndexCarList));
            }

            ViewBag.Year = TempData["DeletedCarYear"];
            ViewBag.Brand = TempData["DeletedCarBrand"];
            ViewBag.Model = TempData["DeletedCarModel"];

            return View();
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
            
            if (Request.Form["IdBrand"] == "__new__") voiture.IdBrand = null;
            if (Request.Form["IdModel"] == "__new__") voiture.IdModel = null;
            if (Request.Form["IdFinishing"] == "__new__") voiture.IdFinishing = null;

            string newBrandName = Request.Form["NewBrandName"];
            string newModelName = Request.Form["NewModelName"];
            string newFinishingName = Request.Form["NewFinishingName"];

           
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

            ModelState.Remove(nameof(Car.IdBrand));
            ModelState.Remove(nameof(Car.IdModel));
            ModelState.Remove(nameof(Car.IdFinishing));

            TryValidateModel(voiture);

            if (ModelState.IsValid)
            {

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
        public async Task<IActionResult> EditCar(int id, Car voiture, IFormFile? imageFile)
        {
            if (id != voiture.Id)
                return NotFound();

            // Gestion des valeurs spéciales "__new__"
            if (Request.Form["IdBrand"] == "__new__") voiture.IdBrand = null;
            if (Request.Form["IdModel"] == "__new__") voiture.IdModel = null;
            if (Request.Form["IdFinishing"] == "__new__") voiture.IdFinishing = null;

            string newBrandName = Request.Form["NewBrandName"];
            string newModelName = Request.Form["NewModelName"];
            string newFinishingName = Request.Form["NewFinishingName"];

            // Création dynamique si nécessaire
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

            // Suppression des anciennes erreurs de validation (lié aux champs mis à jour)
            ModelState.Remove(nameof(Car.IdBrand));
            ModelState.Remove(nameof(Car.IdModel));
            ModelState.Remove(nameof(Car.IdFinishing));
            TryValidateModel(voiture);

            if (!ModelState.IsValid)
            {
                await RemplirViewBags();
                return View(voiture);
            }

            // Gestion de l’image si nouvelle image fournie
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


        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCarConfirmed(int id)
        {
            var car = await _carService.GetByIdAsync(id);
            if (car == null)
                return NotFound();

            await _carService.DeleteAsync(id);

            TempData["DeletedCarYear"] = car.Year;
            TempData["DeletedCarBrand"] = car.Brand?.Name; 
            TempData["DeletedCarModel"] = car.Model?.Name;

            return RedirectToAction(nameof(DeletedConfirmation));
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