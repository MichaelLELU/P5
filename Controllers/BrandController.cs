using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OC_p5_Express_Voitures.Models.Entities;
using OC_p5_Express_Voitures.Services.Interfaces;

public class BrandController : Controller
{
    private readonly IBrandService _brandService;

    public BrandController(IBrandService brandService)
    {
        _brandService = brandService;
    }

    // GET: Brand
    public async Task<IActionResult> Index()
    {
        var brands = await _brandService.GetAllBrandsAsync();
        return View(brands);
    }

    // GET: Brand/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var brand = await _brandService.GetBrandByIdAsync(id);
        if (brand == null)
        {
            return NotFound();
        }
        return View(brand);
    }

    // GET: Brand/Create
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    // POST: Brand/Create
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name")] Brand brand)
    {
        if (ModelState.IsValid)
        {
            await _brandService.CreateBrandAsync(brand);
            return RedirectToAction(nameof(Index));
        }
        return View(brand);
    }
}