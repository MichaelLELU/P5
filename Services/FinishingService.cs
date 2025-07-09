using Microsoft.EntityFrameworkCore;
using OC_p5_Express_Voitures.Data;
using OC_p5_Express_Voitures.Models.Entities;
using OC_p5_Express_Voitures.Services.Interfaces;

namespace OC_p5_Express_Voitures.Services
{
    public class FinishingService : IFinishingService
    {
        private readonly ApplicationDbContext _context;

        public FinishingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Finishing>> GetAllFinishingsAsync()
        {
            return await _context.Finishings.ToListAsync();
        }

        public async Task<Finishing> GetFinishingByIdAsync(int id)
        {
            return await _context.Finishings.FindAsync(id);
        }

        public async Task CreateFinishingAsync(Finishing finishing)
        {
            _context.Finishings.Add(finishing);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Finishing>> GetFinishingsByModelIdAsync(int modelId)
        {
            return await _context.Finishings
                .Where(f => f.ModelId == modelId)
                .ToListAsync();
        }

        public async Task<Finishing> CreateIfNotExistsAsync(string name, int modelId)
        {
            var existing = await _context.Finishings
                .FirstOrDefaultAsync(f =>
                    f.Name.ToLower() == name.ToLower() &&
                    f.ModelId == modelId);

            if (existing != null)
                return existing;

            var newFinishing = new Finishing { Name = name, ModelId = modelId };
            _context.Finishings.Add(newFinishing);
            await _context.SaveChangesAsync();

            return newFinishing;
        }


    }

}
