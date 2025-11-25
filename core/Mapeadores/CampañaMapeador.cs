using MarketingLaPazAPI.Core.Entidades;
using MarketingLaPazAPI.Core.DTOs;

namespace MarketingLaPazAPI.Core.Mapeadores
{
    public static class CampañaMapeador
    {
        public static CampañaDTO ADto(this Campaña campaña)
        {
            return new CampañaDTO
            {
                Id = campaña.Id,
                Nombre = campaña.Nombre,
                FechaInicio = campaña.FechaInicio,
                FechaFin = campaña.FechaFin,
                Presupuesto = campaña.Presupuesto,
                GastosEjecutados = campaña.GastosEjecutados,
                Estado = campaña.Estado,
                TipoCampaña = campaña.TipoCampaña,
                ZonaCobertura = campaña.ZonaCobertura,
                LeadsGenerados = campaña.LeadsGenerados,
                VentasGeneradas = campaña.VentasGeneradas,
                ROI = campaña.ROI
            };
        }

        public static Campaña AEntidad(this CrearCampañaDTO campañaDTO)
        {
            return new Campaña
            {
                Nombre = campañaDTO.Nombre,
                FechaInicio = campañaDTO.FechaInicio,
                FechaFin = campañaDTO.FechaFin,
                Presupuesto = campañaDTO.Presupuesto,
                TipoCampaña = campañaDTO.TipoCampaña,
                ZonaCobertura = campañaDTO.ZonaCobertura,
                Estado = "Activa",
                GastosEjecutados = 0,
                LeadsGenerados = 0,
                VentasGeneradas = 0,
                ROI = 0,
                FechaCreacion = DateTime.UtcNow
            };
        }
    }
}