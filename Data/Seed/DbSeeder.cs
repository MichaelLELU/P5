using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OC_p5_Express_Voitures.Models.Entities;

namespace OC_p5_Express_Voitures.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAdminUserAsync(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            const string adminRole = "Admin";
            const string adminEmail = "lelu.michael@gmail.com";
            const string adminPassword = "P@ssw0rd";

            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, adminRole);
                }
            }
        }

        public static async Task SeedAsync(ApplicationDbContext context)
        {
            await context.Database.MigrateAsync(); 

            var imagesDir = Path.Combine("wwwroot", "cars");
            var seedDir = Path.Combine(imagesDir, "seed");

            if (Directory.Exists(imagesDir))
            {
 
                var filesToDelete = Directory
                    .GetFiles(imagesDir, "*", SearchOption.TopDirectoryOnly) 
                    .ToList();

                foreach (var file in filesToDelete)
                {
                    File.Delete(file);
                }
            }


            // Supprime les données existantes
            context.Cars.RemoveRange(context.Cars);
            context.Finishings.RemoveRange(context.Finishings);
            context.Models.RemoveRange(context.Models);
            context.Brands.RemoveRange(context.Brands);
            context.Reparations.RemoveRange(context.Reparations);
            await context.SaveChangesAsync();


            // 1. Brands
            var brand1 = new Brand { Name = "Toyota" };
                var brand2 = new Brand { Name = "Peugeot" };
                context.Brands.AddRange(brand1, brand2);
                await context.SaveChangesAsync();

                // 2. Models
                var model1 = new Model { Name = "Corolla", Brand = brand1 };
                var model2 = new Model { Name = "Yaris", Brand = brand1 };
                var model3 = new Model { Name = "208", Brand = brand2 };
                var model4 = new Model { Name = "308", Brand = brand2 };
                context.Models.AddRange(model1, model2, model3, model4);
                await context.SaveChangesAsync();

                // 3. Finishings
                var finish1 = new Finishing { Name = "Base", Model = model1 };
                var finish2 = new Finishing { Name = "Premium", Model = model1 };
                var finish3 = new Finishing { Name = "Sport", Model = model2 };
                var finish4 = new Finishing { Name = "Active", Model = model3 };
                var finish5 = new Finishing { Name = "GT Line", Model = model4 };
                var finish6 = new Finishing { Name = "Allure", Model = model4 };
                context.Finishings.AddRange(finish1, finish2, finish3, finish4, finish5, finish6);
                await context.SaveChangesAsync();

                // 4. Reparations
                var repar1 = new Reparation { ReparationType = "Vidange", ReparationPrice = 80 };
                var repar2 = new Reparation { ReparationType = "Pneus", ReparationPrice = 250 };
                var repar3 = new Reparation { ReparationType = "Freins", ReparationPrice = 180 };
                var repar4 = new Reparation { ReparationType = "Climatisation", ReparationPrice = 120 };
                context.Reparations.AddRange(repar1, repar2, repar3, repar4);
                await context.SaveChangesAsync();

                // 5. Cars
                var cars = new List<Car>
        {
            new Car
            {
                CodeVin = "VIN00000000000001",
                Year = 2022,
                Brand = brand1,
                Model = model1,
                Finishing = finish1,
                PurchasePrice = 18000,
                AvailabilityDate = DateTime.Now.AddDays(30),
                Reparations = new List<Reparation> { repar1, repar2 },
                ImagePath = "seed/corolla-base-v3.png"
            },
            new Car
            {
                CodeVin = "VIN00000000000002",
                Year = 2023,
                Brand = brand1,
                Model = model1,
                Finishing = finish2,
                PurchasePrice = 19000,
                AvailabilityDate = DateTime.Now.AddDays(25),
                Reparations = new List<Reparation> { repar3 },
                ImagePath = "seed/corolla-premium.png"
            },
            new Car
            {
                CodeVin = "VIN00000000000003",
                Year = 2021,
                Brand = brand1,
                Model = model2,
                Finishing = finish3,
                PurchasePrice = 17000,
                AvailabilityDate = DateTime.Now.AddDays(15),
                Reparations = new List<Reparation> { repar4 },
                ImagePath = "seed/toyota-yaris-sport.jpg"
            },
            new Car
            {
                CodeVin = "VIN00000000000004",
                Year = 2023,
                Brand = brand2,
                Model = model3,
                Finishing = finish4,
                PurchasePrice = 21000,
                AvailabilityDate = DateTime.Now.AddDays(10),
                Reparations = new List<Reparation> { repar1, repar3 },
                ImagePath = "seed/peugeot-208-active.jpg"
            },
            new Car
            {
                CodeVin = "VIN00000000000005",
                Year = 2022,
                Brand = brand2,
                Model = model4,
                Finishing = finish5,
                PurchasePrice = 23000,
                AvailabilityDate = DateTime.Now.AddDays(5),
                Reparations = new List<Reparation> { repar2 },
                ImagePath = "seed/peugeot-308-gt.jpg"
            },
            new Car
            {
                CodeVin = "VIN00000000000006",
                Year = 2024,
                Brand = brand2,
                Model = model4,
                Finishing = finish6,
                PurchasePrice = 25000,
                AvailabilityDate = DateTime.Now.AddDays(2),
                Reparations = new List<Reparation> { repar4 },
                ImagePath = "seed/peugeot-308-allure.jpeg"
            }
        };

                context.Cars.AddRange(cars);
                await context.SaveChangesAsync();
            }
        }


    }

