using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OC_p5_Express_Voitures.Data;
using OC_p5_Express_Voitures.Services;
using OC_p5_Express_Voitures.Services.Interfaces;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("TestProjectCarExpress")]


var builder = WebApplication.CreateBuilder(args);

// ----- Configuration de la connexion et EF Core -----
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// ----- Identity -----
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;   // TODO a changer pour mise en prod 
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

// ----- Services personnalis�s -----
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IReparationService, ReparationService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<IModelService, ModelService>();
builder.Services.AddScoped<IFinishingService, FinishingService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// ----- Seeding des donn�es + admin -----
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

    await DbSeeder.SeedAsync(context); // seed donn�es
    await DbSeeder.SeedAdminUserAsync(roleManager, userManager); // seed admin
}

// ----- Middleware pipeline -----
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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
    pattern: "{controller=Car}/{action=IndexCarList}/{id?}");

app.MapRazorPages();

app.Run();

public partial class Program { }