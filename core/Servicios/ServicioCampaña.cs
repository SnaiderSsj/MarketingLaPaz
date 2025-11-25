using MarketingLaPazAPI.Core.Interfaces;
using MarketingLaPazAPI.Core.Entidades;
using MarketingLaPazAPI.Core.DTOs;
using MarketingLaPazAPI.Core.Mapeadores;

namespace MarketingLaPazAPI.Core.Servicios
{
    public class ServicioCampaña : IServicioCampaña
    {
        private readonly ICampañaRepositorio _campañaRepositorio;

        public ServicioCampaña(ICampañaRepositorio campañaRepositorio)
        {
            _campañaRepositorio = campañaRepositorio;
        }

        public async Task<IEnumerable<CampañaDTO>> ObtenerTodasLasCampañasAsync()
        {
            var campañas = await _campañaRepositorio.ObtenerTodosAsync();
            return campañas.Select(c => c.ADto());
        }

        public async Task<CampañaDTO> ObtenerCampañaPorIdAsync(int id)
        {
            var campaña = await _campañaRepositorio.ObtenerPorIdAsync(id);
            return campaña?.ADto();
        }

        public async Task<CampañaDTO> CrearCampañaAsync(CrearCampañaDTO campañaDTO)
        {
            var campaña = campañaDTO.AEntidad();
            var campañaCreada = await _campañaRepositorio.AgregarAsync(campaña);
            return campañaCreada.ADto();
        }

        public async Task<CampañaDTO> ActualizarCampañaAsync(int id, ActualizarCampañaDTO campañaDTO)
        {
            var campañaExistente = await _campañaRepositorio.ObtenerPorIdAsync(id);
            if (campañaExistente == null)
                return null;

            campañaExistente.Nombre = campañaDTO.Nombre;
            campañaExistente.FechaFin = campañaDTO.FechaFin;
            campañaExistente.Presupuesto = campañaDTO.Presupuesto;
            campañaExistente.Estado = campañaDTO.Estado;

            await _campañaRepositorio.ActualizarAsync(campañaExistente);
            return campañaExistente.ADto();
        }

        public async Task<bool> EliminarCampañaAsync(int id)
        {
            var campaña = await _campañaRepositorio.ObtenerPorIdAsync(id);
            if (campaña == null)
                return false;

            await _campañaRepositorio.EliminarAsync(campaña);
            return true;
        }

        public async Task<IEnumerable<CampañaDTO>> ObtenerCampañasActivasAsync()
        {
            var campañasActivas = await _campañaRepositorio.ObtenerCampañasActivasAsync();
            return campañasActivas.Select(c => c.ADto());
        }

        public async Task<object> ObtenerEstadisticasCampañasAsync()
        {
            var campañas = await _campañaRepositorio.ObtenerTodosAsync();
            var presupuestoTotal = await _campañaRepositorio.ObtenerPresupuestoTotalAsync();

            return new
            {
                TotalCampañas = campañas.Count(),
                CampañasActivas = campañas.Count(c => c.Estado == "Activa"),
                PresupuestoTotal = presupuestoTotal,
                ROIPromedio = campañas.Any() ? campañas.Average(c => c.ROI) : 0,
                CampañasPorEstado = campañas.GroupBy(c => c.Estado)
                    .ToDictionary(g => g.Key, g => g.Count())
            };
        }
    }
}