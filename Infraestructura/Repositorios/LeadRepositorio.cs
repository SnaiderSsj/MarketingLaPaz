using Microsoft.EntityFrameworkCore;
using MarketingLaPazAPI.Core.Entidades;
using MarketingLaPazAPI.Core.Interfaces;
using MarketingLaPazAPI.Infraestructura.Data;

namespace MarketingLaPazAPI.Infraestructura.Repositorios
{
    public class LeadRepositorio : RepositorioBase<Lead>, ILeadRepositorio
    {
        public LeadRepositorio(MarketingDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Lead>> ObtenerLeadsPorCampañaAsync(int campañaId)
        {
            return await _context.Leads
                .Include(l => l.Campaña)
                .Where(l => l.CampañaId == campañaId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Lead>> ObtenerLeadsPorEstadoAsync(string estado)
        {
            return await _context.Leads
                .Include(l => l.Campaña)
                .Where(l => l.Estado == estado)
                .ToListAsync();
        }

        public async Task<int> ContarLeadsPorCampañaAsync(int campañaId)
        {
            return await _context.Leads
                .CountAsync(l => l.CampañaId == campañaId);
        }

        public async Task<Dictionary<string, int>> ObtenerEstadisticasLeadsAsync()
        {
            return await _context.Leads
                .GroupBy(l => l.Estado)
                .Select(g => new { Estado = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Estado, x => x.Count);
        }

        public override async Task<IEnumerable<Lead>> ObtenerTodosAsync()
        {
            return await _context.Leads
                .Include(l => l.Campaña)
                .ToListAsync();
        }

        public override async Task<Lead> ObtenerPorIdAsync(int id)
        {
            return await _context.Leads
                .Include(l => l.Campaña)
                .FirstOrDefaultAsync(l => l.Id == id);
        }
    }
}