using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OC_p5_Express_Voitures.Models.Entities;

namespace OC_p5_Express_Voitures.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Finishing> Finishings { get; set; }
        public DbSet<Reparation> Reparations { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // === Contrainte Car ===
            modelBuilder.Entity<Car>(entity =>
            {
                entity.HasIndex(c => c.CodeVin).IsUnique();
                entity.Property(c => c.CodeVin).IsRequired().HasMaxLength(17);
                entity.Property(c => c.Year).IsRequired();
                entity.Property(c => c.PurchasePrice).HasColumnType("decimal(10,2)");
                entity.Property(c => c.SalePrice).HasColumnType("decimal(10,2)");
                entity.Property(c => c.PurchaseDate).HasDefaultValueSql("GETDATE()");
            });

            // === Brand ===
            modelBuilder.Entity<Brand>(entity =>
            {
                entity.Property(b => b.Name).IsRequired().HasMaxLength(100);
            });

            // === Model ===
            modelBuilder.Entity<Model>(entity =>
            {
                entity.Property(m => m.Name).IsRequired().HasMaxLength(100);
            });

            // === Finishing ===
            modelBuilder.Entity<Finishing>(entity =>
            {
                entity.Property(f => f.Name).IsRequired().HasMaxLength(100);
            });

            // === Reparation ===
            modelBuilder.Entity<Reparation>(entity =>
            {
                entity.Property(r => r.ReparationType).IsRequired().HasMaxLength(250);
                entity.Property(r => r.ReparationPrice).HasColumnType("decimal(10,2)");
            });

            // === Relations ===

            modelBuilder.Entity<Model>()
                .HasOne(m => m.Brand)
                .WithMany(b => b.Models)
                .HasForeignKey(m => m.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Finishing>()
                .HasOne(f => f.Model)
                .WithMany(m => m.Finishings)
                .HasForeignKey(f => f.ModelId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Car>()
                .HasOne(c => c.Brand)
                .WithMany(b => b.Cars)
                .HasForeignKey(c => c.IdBrand)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Car>()
                .HasOne(c => c.Model)
                .WithMany(m => m.Cars)
                .HasForeignKey(c => c.IdModel)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Car>()
                .HasOne(c => c.Finishing)
                .WithMany(f => f.Cars)
                .HasForeignKey(c => c.IdFinishing)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reparation>()
                .HasMany(r => r.Cars)
                .WithMany(c => c.Reparations);
        }
    }
}
