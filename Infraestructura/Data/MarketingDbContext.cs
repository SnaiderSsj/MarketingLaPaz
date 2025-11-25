using Microsoft.EntityFrameworkCore;
using MarketingLaPazAPI.Core.Entidades;

namespace MarketingLaPazAPI.Infraestructura.Data
{
    public class MarketingDbContext : DbContext
    {
        public MarketingDbContext(DbContextOptions<MarketingDbContext> options) : base(options)
        {
        }

        public DbSet<Campaña> Campañas { get; set; }
        public DbSet<Lead> Leads { get; set; }
        public DbSet<PresupuestoMarketing> Presupuestos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración para PostgreSQL y relaciones
            modelBuilder.Entity<Campaña>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Nombre).IsUnique();
                entity.Property(e => e.Nombre).HasMaxLength(200).IsRequired();
                entity.Property(e => e.Estado).HasMaxLength(50);
                entity.Property(e => e.TipoCampaña).HasMaxLength(100);
            });

            modelBuilder.Entity<Lead>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email);
                entity.HasOne(e => e.Campaña)
                    .WithMany(c => c.Leads)
                    .HasForeignKey(e => e.CampañaId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PresupuestoMarketing>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.Año, e.Mes, e.Departamento }).IsUnique();
            });
        }
    }
}