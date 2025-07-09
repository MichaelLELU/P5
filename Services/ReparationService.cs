using Microsoft.EntityFrameworkCore;
using OC_p5_Express_Voitures.Data;
using OC_p5_Express_Voitures.Models.Entities;
using OC_p5_Express_Voitures.Services.Interfaces;

namespace OC_p5_Express_Voitures.Services
{
    public class ReparationService : IReparationService
    {
        private readonly ApplicationDbContext _context;

        public ReparationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Reparation>> GetAllAsync()
        {
            return await _context.Reparations.ToListAsync();
        }

        public async Task<Reparation?> GetByIdAsync(int id)
        {
            return await _context.Reparations.FindAsync(id);
        }

        public async Task CreateAsync(Reparation reparation)
        {
            _context.Add(reparation);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Reparation reparation)
        {
            _context.Update(reparation);
            await _context.SaveChangesAsync();
        }


        public bool Exists(int id)
        {
            return _context.Reparations.Any(r => r.Id == id);
        }
    }
}

