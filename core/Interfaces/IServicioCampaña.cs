using MarketingLaPazAPI.Core.DTOs;

namespace MarketingLaPazAPI.Core.Interfaces
{
    public interface IServicioCampaña
    {
        Task<IEnumerable<CampañaDTO>> ObtenerTodasLasCampañasAsync();
        Task<CampañaDTO> ObtenerCampañaPorIdAsync(int id);
        Task<CampañaDTO> CrearCampañaAsync(CrearCampañaDTO campañaDTO);
        Task<CampañaDTO> ActualizarCampañaAsync(int id, ActualizarCampañaDTO campañaDTO);
        Task<bool> EliminarCampañaAsync(int id);
        Task<IEnumerable<CampañaDTO>> ObtenerCampañasActivasAsync();
        Task<object> ObtenerEstadisticasCampañasAsync();
    }
}