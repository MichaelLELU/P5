using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OC_p5_Express_Voitures.Models.Entities;
using OC_p5_Express_Voitures.Services.Interfaces;

public class FinishingController : Controller
{
    private readonly IFinishingService _finishingService;

    public FinishingController(IFinishingService finishingService)
    {
        _finishingService = finishingService;
    }

    // GET: Finishing
    public async Task<IActionResult> Index()
    {
        var finishings = await _finishingService.GetAllFinishingsAsync();
        return View(finishings);
    }

    // GET: Finishing/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var finishing = await _finishingService.GetFinishingByIdAsync(id);
        if (finishing == null)
        {
            return NotFound();
        }
        return View(finishing);
    }

    // GET: Finishing/Create
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    // POST: Finishing/Create
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name")] Finishing finishing)
    {
        if (ModelState.IsValid)
        {
            await _finishingService.CreateFinishingAsync(finishing);
            return RedirectToAction(nameof(Index));
        }
        return View(finishing);
    }
}