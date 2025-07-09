using System.ComponentModel.DataAnnotations;

namespace OC_p5_Express_Voitures.Models.Entities
{
    public class Finishing
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public int ModelId { get; set; }
        public Model Model { get; set; }

        public ICollection<Car> Cars { get; set; } = new List<Car>();
    }
}
