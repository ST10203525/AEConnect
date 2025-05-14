using AEConnect.Data;
using AEConnect.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Employee")]
public class FarmerController : Controller
{
    private readonly ApplicationDbContext _context;

    public FarmerController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Farmer
    public async Task<IActionResult> Index()
    {
        return View(await _context.Farmers.ToListAsync());
    }

    // GET: Farmer/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Farmer/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Farmer farmer)
    {
        if (ModelState.IsValid)
        {
            _context.Add(farmer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(farmer);
    }

    // GET: Farmer/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var farmer = await _context.Farmers
            .Include(f => f.Products)
            .FirstOrDefaultAsync(m => m.FarmerId == id);

        if (farmer == null) return NotFound();

        return View(farmer);
    }
}
