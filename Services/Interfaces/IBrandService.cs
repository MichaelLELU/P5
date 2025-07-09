using OC_p5_Express_Voitures.Models.Entities;

namespace OC_p5_Express_Voitures.Services.Interfaces
{
    public interface IBrandService
    {
        Task<IEnumerable<Brand>> GetAllBrandsAsync();
        Task<Brand> GetBrandByIdAsync(int id);
        Task CreateBrandAsync(Brand brand);
        Task<Brand> CreateIfNotExistsAsync(string name);

    }

}
