using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OC_p5_Express_Voitures.Models.Entities
{
    public class Model
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public int BrandId { get; set; }
        public Brand Brand { get; set; }

        public ICollection<Finishing> Finishings { get; set; } = new List<Finishing>();

        public ICollection<Car> Cars { get; set; } = new List<Car>();
    }
}
