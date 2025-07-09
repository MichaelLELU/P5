using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OC_p5_Express_Voitures.Models.Entities;
using OC_p5_Express_Voitures.Services.Interfaces;

public class ModelController : Controller
{
    private readonly IModelService _modelService;

    public ModelController(IModelService modelService)
    {
        _modelService = modelService;
    }

    // GET: Model
    public async Task<IActionResult> Index()
    {
        var models = await _modelService.GetAllModelsAsync();
        return View(models);
    }

    // GET: Model/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var model = await _modelService.GetModelByIdAsync(id);
        if (model == null)
        {
            return NotFound();
        }
        return View(model);
    }

    // GET: Model/Create
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    // POST: Model/Create
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name")] Model model)
    {
        if (ModelState.IsValid)
        {
            await _modelService.CreateModelAsync(model);
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }
}