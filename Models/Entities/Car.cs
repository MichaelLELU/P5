using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OC_p5_Express_Voitures.Models.Entities
{

    public class Car
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "code VIN unique obligatoire")]
        [StringLength(17)]
        public string CodeVin { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        [ForeignKey(nameof(Brand))]
        public int? IdBrand { get; set; }
        public Brand? Brand { get; set; }

        [Required]
        [ForeignKey(nameof(Model))]
        public int? IdModel { get; set; }
        public Model? Model { get; set; }

        [Required]
        [ForeignKey(nameof(Finishing))]
        public int? IdFinishing { get; set; }
        public Finishing? Finishing { get; set; }

        public DateTime AvailabilityDate { get; set; }

        public DateTime PurchaseDate { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(10,2)")]
        public decimal PurchasePrice { get; set; }

        public DateTime? SaleDate { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? SalePrice { get; set; }

        [MaxLength(255)]
        public string? ImagePath { get; set; }

        public ICollection<Reparation> Reparations { get; set; } = new List<Reparation>();
    }
}
