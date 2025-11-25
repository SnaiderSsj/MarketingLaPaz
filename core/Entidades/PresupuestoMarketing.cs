namespace MarketingLaPazAPI.Core.Entidades
{
    public class PresupuestoMarketing
    {
        public int Id { get; set; }
        public int Año { get; set; }
        public int Mes { get; set; }
        public decimal PresupuestoAsignado { get; set; }
        public decimal GastosEjecutados { get; set; } = 0;
        public string Departamento { get; set; } = "Marketing La Paz";
        public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;
    }
}