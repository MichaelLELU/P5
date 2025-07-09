using OC_p5_Express_Voitures.Models.Entities;

namespace OC_p5_Express_Voitures.Services.Interfaces
{
    public interface ICarService
    {
        Task<List<Car>> GetAllAsync();
        Task<Car?> GetByIdAsync(int id);
        Task CreateAsync(Car voiture);
        Task UpdateAsync(Car voiture);
        Task DeleteAsync(int id);
        bool Exists(int id);
    }
}