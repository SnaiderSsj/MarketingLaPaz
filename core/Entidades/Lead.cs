namespace MarketingLaPazAPI.Core.Entidades
{
    public class Lead
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Empresa { get; set; }
        public string Cargo { get; set; }
        public string Origen { get; set; } = "Web";
        public string Estado { get; set; } = "Nuevo";
        public int CampañaId { get; set; }
        public Campaña Campaña { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public string Notas { get; set; }
    }
}