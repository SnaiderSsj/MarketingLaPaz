namespace MarketingLaPazAPI.Core.DTOs
{
    public class LeadDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Empresa { get; set; }
        public string Cargo { get; set; }
        public string Origen { get; set; }
        public string Estado { get; set; }
        public int CampañaId { get; set; }
        public string CampañaNombre { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class CrearLeadDTO
    {
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Empresa { get; set; }
        public string Cargo { get; set; }
        public string Origen { get; set; }
        public int CampañaId { get; set; }
        public string Notas { get; set; }
    }
}