using System.ComponentModel.DataAnnotations;

namespace OC_p5_Express_Voitures.Models.Entities
{
    public class Brand
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public ICollection<Model> Models { get; set; } = new List<Model>();
        public ICollection<Car> Cars { get; set; } = new List<Car>();
    }
}
