using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AEConnect.Data;
using AEConnect.Models;

[Authorize(Roles = "Farmer")]
public class ProductController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public ProductController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: Product/Add
    public async Task<IActionResult> Add()
    {
        // Ensure the Farmer exists
        var email = User.Identity?.Name;
        var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.Email == email);
        if (farmer == null)
        {
            return RedirectToAction("Create", "Farmer"); // ask user to create their profile
        }

        return View();
    }

    // POST: Product/Add
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(Product product)
    {
        var email = User.Identity?.Name;
        var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.Email == email);
        if (farmer == null)
        {
            return RedirectToAction("Create", "Farmer");
        }

        if (ModelState.IsValid)
        {
            product.FarmerId = farmer.FarmerId;
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MyProducts));
        }

        return View(product);
    }

    // GET: Product/MyProducts
    public async Task<IActionResult> MyProducts()
    {
        var email = User.Identity?.Name;
        var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.Email == email);
        if (farmer == null)
        {
            return RedirectToAction("Create", "Farmer");
        }

        var products = await _context.Products
            .Where(p => p.FarmerId == farmer.FarmerId)
            .ToListAsync();

        return View(products);
    }
}
