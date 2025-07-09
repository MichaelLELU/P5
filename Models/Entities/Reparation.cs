using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OC_p5_Express_Voitures.Models.Entities
{
    public class Reparation
    {
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string ReparationType { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal ReparationPrice { get; set; }

        public ICollection<Car> Cars { get; set; } = new List<Car>();
    }

}
