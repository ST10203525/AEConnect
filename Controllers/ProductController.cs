using AEConnect.Data;
using AEConnect.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AEConnect.Controllers
{
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

        // Show form to add product
        public async Task<IActionResult> Add()
        {
            var email = User.Identity?.Name;
            var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.Email == email);
            if (farmer == null)
            {
                return RedirectToAction("Create", "Farmer");
            }

            return View();
        }

        // Process form submission
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
                return RedirectToAction("MyProducts");
            }

            return View(product);
        }

        // View own products
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
}
