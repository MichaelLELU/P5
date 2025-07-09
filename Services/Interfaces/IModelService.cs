using OC_p5_Express_Voitures.Models.Entities;

namespace OC_p5_Express_Voitures.Services.Interfaces
{
    public interface IModelService
    {
        Task<IEnumerable<Model>> GetAllModelsAsync();
        Task<Model> GetModelByIdAsync(int id);
        Task CreateModelAsync(Model model);
        Task<IEnumerable<Model>> GetModelsByBrandIdAsync(int brandId);
        Task<Model> CreateIfNotExistsAsync(string name, int brandId);


    }

}
