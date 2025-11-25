using MarketingLaPazAPI.Core.Entidades;
using MarketingLaPazAPI.Core.DTOs;

namespace MarketingLaPazAPI.Core.Mapeadores
{
    public static class LeadMapeador
    {
        public static LeadDTO ADto(this Lead lead)
        {
            return new LeadDTO
            {
                Id = lead.Id,
                Nombre = lead.Nombre,
                Email = lead.Email,
                Telefono = lead.Telefono,
                Empresa = lead.Empresa,
                Cargo = lead.Cargo,
                Origen = lead.Origen,
                Estado = lead.Estado,
                CampañaId = lead.CampañaId,
                CampañaNombre = lead.Campaña?.Nombre,
                FechaCreacion = lead.FechaCreacion
            };
        }

        public static Lead AEntidad(this CrearLeadDTO leadDTO)
        {
            return new Lead
            {
                Nombre = leadDTO.Nombre,
                Email = leadDTO.Email,
                Telefono = leadDTO.Telefono,
                Empresa = leadDTO.Empresa,
                Cargo = leadDTO.Cargo,
                Origen = leadDTO.Origen,
                Estado = "Nuevo",
                CampañaId = leadDTO.CampañaId,
                Notas = leadDTO.Notas,
                FechaCreacion = DateTime.UtcNow
            };
        }
    }
}