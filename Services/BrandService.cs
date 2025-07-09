using OC_p5_Express_Voitures.Data;
using Microsoft.EntityFrameworkCore;
using OC_p5_Express_Voitures.Models.Entities;
using OC_p5_Express_Voitures.Services.Interfaces;

namespace OC_p5_Express_Voitures.Services
{
    public class BrandService : IBrandService
    {
        private readonly ApplicationDbContext _context;

        public BrandService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Brand>> GetAllBrandsAsync()
        {
            return await _context.Brands.ToListAsync();
        }

        public async Task<Brand> GetBrandByIdAsync(int id)
        {
            return await _context.Brands.FindAsync(id);
        }

        public async Task CreateBrandAsync(Brand brand)
        {
            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();
        }

        public async Task<Brand> CreateIfNotExistsAsync(string name)
        {
            var existing = await _context.Brands
                .FirstOrDefaultAsync(b => b.Name.ToLower() == name.ToLower());

            if (existing != null)
                return existing;

            var newBrand = new Brand { Name = name };
            _context.Brands.Add(newBrand);
            await _context.SaveChangesAsync();

            return newBrand;
        }


    }

}
