using OC_p5_Express_Voitures.Models.Entities;

namespace OC_p5_Express_Voitures.Services.Interfaces
{
    public interface IFinishingService
    {
        Task<IEnumerable<Finishing>> GetAllFinishingsAsync();
        Task<Finishing> GetFinishingByIdAsync(int id);
        Task CreateFinishingAsync(Finishing finishing);
        Task<IEnumerable<Finishing>> GetFinishingsByModelIdAsync(int modelId);
        Task<Finishing> CreateIfNotExistsAsync(string name, int modelId);
    }

}
