using Microsoft.EntityFrameworkCore;
using OC_p5_Express_Voitures.Data;
using OC_p5_Express_Voitures.Models.Entities;
using OC_p5_Express_Voitures.Services.Interfaces;

namespace OC_p5_Express_Voitures.Services
{
    public class ModelService : IModelService
    {
        private readonly ApplicationDbContext _context;

        public ModelService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Model>> GetAllModelsAsync()
        {
            return await _context.Models.ToListAsync();
        }

        public async Task<Model> GetModelByIdAsync(int id)
        {
            return await _context.Models.FindAsync(id);
        }

        public async Task CreateModelAsync(Model model)
        {
            _context.Models.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Model>> GetModelsByBrandIdAsync(int brandId)
        {
            return await _context.Models
                .Where(m => m.BrandId == brandId)
                .ToListAsync();
        }

        public async Task<Model> CreateIfNotExistsAsync(string name, int brandId)
        {
            var existing = await _context.Models
                .FirstOrDefaultAsync(m =>
                    m.Name.ToLower() == name.ToLower() &&
                    m.BrandId == brandId);

            if (existing != null)
                return existing;

            var newModel = new Model { Name = name, BrandId = brandId };
            _context.Models.Add(newModel);
            await _context.SaveChangesAsync();

            return newModel;
        }



    }

}
