using Microsoft.EntityFrameworkCore;
using MarketingLaPazAPI.Core.Entidades;
using MarketingLaPazAPI.Core.Interfaces;
using MarketingLaPazAPI.Infraestructura.Data;

namespace MarketingLaPazAPI.Infraestructura.Repositorios
{
    public class CampañaRepositorio : RepositorioBase<Campaña>, ICampañaRepositorio
    {
        public CampañaRepositorio(MarketingDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Campaña>> ObtenerCampañasActivasAsync()
        {
            return await _context.Campañas
                .Where(c => c.Estado == "Activa" && c.FechaFin >= DateTime.UtcNow)
                .ToListAsync();
        }

        public async Task<IEnumerable<Campaña>> ObtenerCampañasPorEstadoAsync(string estado)
        {
            return await _context.Campañas
                .Where(c => c.Estado == estado)
                .ToListAsync();
        }

        public async Task<Campaña> ActualizarROIAsync(int campañaId, decimal ventasGeneradas)
        {
            var campaña = await _context.Campañas.FindAsync(campañaId);
            if (campaña != null)
            {
                campaña.VentasGeneradas = ventasGeneradas;
                campaña.ROI = campaña.Presupuesto > 0 ?
                    (ventasGeneradas - campaña.Presupuesto) / campaña.Presupuesto * 100 : 0;

                await _context.SaveChangesAsync();
            }
            return campaña;
        }

        public async Task<decimal> ObtenerPresupuestoTotalAsync()
        {
            return await _context.Campañas
                .Where(c => c.Estado == "Activa")
                .SumAsync(c => c.Presupuesto);
        }

        public override async Task<Campaña> ObtenerPorIdAsync(int id)
        {
            return await _context.Campañas
                .Include(c => c.Leads)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}