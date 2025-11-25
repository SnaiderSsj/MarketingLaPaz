namespace MarketingLaPazAPI.Core.DTOs
{
    public class CampañaDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal Presupuesto { get; set; }
        public decimal GastosEjecutados { get; set; }
        public string Estado { get; set; }
        public string TipoCampaña { get; set; }
        public string ZonaCobertura { get; set; }
        public int LeadsGenerados { get; set; }
        public decimal VentasGeneradas { get; set; }
        public decimal ROI { get; set; }
    }

    public class CrearCampañaDTO
    {
        public string Nombre { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal Presupuesto { get; set; }
        public string TipoCampaña { get; set; }
        public string ZonaCobertura { get; set; }
    }

    public class ActualizarCampañaDTO
    {
        public string Nombre { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal Presupuesto { get; set; }
        public string Estado { get; set; }
    }
}