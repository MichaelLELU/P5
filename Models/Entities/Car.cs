using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OC_p5_Express_Voitures.Models.Entities
{
    public class Car
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le code VIN est obligatoire.")]
        [StringLength(17, MinimumLength = 17, ErrorMessage = "Le code VIN doit contenir exactement 17 caractères.")]
        public string CodeVin { get; set; }

        [Required(ErrorMessage = "L'année de mise en circulation est obligatoire.")]
        [Range(1800, 3000, ErrorMessage = "L'année doit être comprise entre 1800 et l'année en cours.")]
        public int Year { get; set; }

        [Required(ErrorMessage = "La marque est obligatoire.")]
        [ForeignKey(nameof(Brand))]
        public int? IdBrand { get; set; }
        public Brand? Brand { get; set; }

        [Required(ErrorMessage = "Le modèle est obligatoire.")]
        [ForeignKey(nameof(Model))]
        public int? IdModel { get; set; }
        public Model? Model { get; set; }

        [Required(ErrorMessage = "La finition est obligatoire.")]
        [ForeignKey(nameof(Finishing))]
        public int? IdFinishing { get; set; }
        public Finishing? Finishing { get; set; }

        [DataType(DataType.Date)]
        public DateTime AvailabilityDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime PurchaseDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Le prix d'achat est obligatoire.")]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Le prix d'achat doit être supérieur à 0.")]
        public decimal PurchasePrice { get; set; }

        [DataType(DataType.Date)]
        public DateTime? SaleDate { get; set; }

        [Required(ErrorMessage = "Le prix de vente est obligatoire.")]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Le prix de vente doit être supérieur à 0.")]
        public decimal? SalePrice { get; set; }

        [MaxLength(300, ErrorMessage = "Le nom du fichier image est trop long.")]
        public string? ImagePath { get; set; }

        public ICollection<Reparation> Reparations { get; set; } = new List<Reparation>();
    }
}
