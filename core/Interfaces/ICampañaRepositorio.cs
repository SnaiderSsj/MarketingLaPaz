using MarketingLaPazAPI.Core.Entidades;

namespace MarketingLaPazAPI.Core.Interfaces
{
    public interface ICampañaRepositorio : IRepositorio<Campaña>
    {
        Task<IEnumerable<Campaña>> ObtenerCampañasActivasAsync();
        Task<IEnumerable<Campaña>> ObtenerCampañasPorEstadoAsync(string estado);
        Task<Campaña> ActualizarROIAsync(int campañaId, decimal ventasGeneradas);
        Task<decimal> ObtenerPresupuestoTotalAsync();
    }
}