using AEConnect.Data;
using AEConnect.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity with Roles
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Seed roles, users, farmers, and products
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
    var context = services.GetRequiredService<ApplicationDbContext>();

    // Ensure database exists
    context.Database.EnsureCreated();

    // Create roles
    string[] roles = { "Farmer", "Employee" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Seed a Farmer user
    var farmerEmail = "farmer@example.com";
    var farmerUser = await userManager.FindByEmailAsync(farmerEmail);
    if (farmerUser == null)
    {
        farmerUser = new IdentityUser { UserName = farmerEmail, Email = farmerEmail, EmailConfirmed = true };
        var result = await userManager.CreateAsync(farmerUser, "Farmer@123");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(farmerUser, "Farmer");
        }
    }

    // Seed Farmer profile
    var farmerProfile = await context.Farmers.FirstOrDefaultAsync(f => f.Email == farmerEmail);
    if (farmerProfile == null)
    {
        farmerProfile = new Farmer
        {
            FullName = "John Green",
            Email = farmerEmail,
            PhoneNumber = "0123456789",
            Location = "Limpopo"
        };
        context.Farmers.Add(farmerProfile);
        await context.SaveChangesAsync();
    }

    // Seed an Employee user
    var employeeEmail = "employee@example.com";
    var employeeUser = await userManager.FindByEmailAsync(employeeEmail);
    if (employeeUser == null)
    {
        employeeUser = new IdentityUser { UserName = employeeEmail, Email = employeeEmail, EmailConfirmed = true };
        var result = await userManager.CreateAsync(employeeUser, "Employee@123");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(employeeUser, "Employee");
        }
    }

    // Seed sample products for the farmer
    if (!context.Products.Any())
    {
        var products = new List<Product>
        {
            new Product
            {
                ProductName = "Organic Maize",
                Category = "Grain",
                ProductionDate = new DateTime(2025, 1, 15),
                FarmerId = farmerProfile.FarmerId
            },
            new Product
            {
                ProductName = "Free-Range Eggs",
                Category = "Poultry",
                ProductionDate = new DateTime(2025, 3, 5),
                FarmerId = farmerProfile.FarmerId
            },
            new Product
            {
                ProductName = "Solar-Powered Drip Kit",
                Category = "Green Tech",
                ProductionDate = new DateTime(2025, 2, 20),
                FarmerId = farmerProfile.FarmerId
            }
        };

        context.Products.AddRange(products);
        await context.SaveChangesAsync();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
