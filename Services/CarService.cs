using Microsoft.EntityFrameworkCore;
using OC_p5_Express_Voitures.Data;
using OC_p5_Express_Voitures.Models.Entities;
using OC_p5_Express_Voitures.Services.Interfaces;

namespace OC_p5_Express_Voitures.Services
{
    public class CarService : ICarService
    {
        private readonly ApplicationDbContext _context;

        public CarService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Car>> GetAllAsync()
        {
            return await _context.Cars
                .Include(c => c.Brand)
                .Include(c => c.Model)
                .Include(c => c.Finishing)
                .ToListAsync();
        }

        public async Task<Car?> GetByIdAsync(int id)
        {
            return await _context.Cars
                .Include(c => c.Brand)
                .Include(c => c.Model)
                .Include(c => c.Finishing)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task CreateAsync(Car voiture)
        {
            _context.Add(voiture);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Car voiture)
        {
            _context.Update(voiture);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();
            }
        }

        public bool Exists(int id)
        {
            return _context.Cars.Any(v => v.Id == id);
        }
    }
}