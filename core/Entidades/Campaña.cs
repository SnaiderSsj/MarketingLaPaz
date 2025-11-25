using System.ComponentModel.DataAnnotations;

namespace MarketingLaPazAPI.Core.Entidades
{
    public class Campaña
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Nombre { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        [Required]
        public DateTime FechaFin { get; set; }

        [Required]
        public decimal Presupuesto { get; set; }

        public decimal GastosEjecutados { get; set; }

        [StringLength(50)]
        public string Estado { get; set; } = "Activa";

        [StringLength(100)]
        public string TipoCampaña { get; set; }

        public string ZonaCobertura { get; set; }

        public int LeadsGenerados { get; set; }

        public decimal VentasGeneradas { get; set; }

        public decimal ROI { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual ICollection<Lead> Leads { get; set; } = new List<Lead>();
    }
}