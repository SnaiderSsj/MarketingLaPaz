using MarketingLaPazAPI.Core.Entidades;

namespace MarketingLaPazAPI.Core.Interfaces
{
    public interface ILeadRepositorio : IRepositorio<Lead>
    {
        Task<IEnumerable<Lead>> ObtenerLeadsPorCampañaAsync(int campañaId);
        Task<IEnumerable<Lead>> ObtenerLeadsPorEstadoAsync(string estado);
        Task<int> ContarLeadsPorCampañaAsync(int campañaId);
        Task<Dictionary<string, int>> ObtenerEstadisticasLeadsAsync();
    }
}