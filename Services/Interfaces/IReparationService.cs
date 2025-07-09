using OC_p5_Express_Voitures.Models.Entities;

namespace OC_p5_Express_Voitures.Services.Interfaces
{
    public interface IReparationService
    {
        Task<List<Reparation>> GetAllAsync();
        Task<Reparation?> GetByIdAsync(int id);
        Task CreateAsync(Reparation reparation);
        Task UpdateAsync(Reparation reparation);
        bool Exists(int id);
    }
}