using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MarketingLaPazAPI.Infraestructura.Data;
using MarketingLaPazAPI.Core.Entidades;

namespace MarketingLaPazAPI.Presentacion.Controladores
{
    [Route("marketing/la-paz")]
    [ApiController]
    public class MarketingLaPazController : ControllerBase
    {
        private readonly MarketingDbContext _context;

        public MarketingLaPazController(MarketingDbContext context)
        {
            _context = context;
        }

        // ENDPOINTS QUE MARKETING PROPORCIONA A OTROS DEPARTAMENTOS

        [HttpGet("roi-campañas")]
        public async Task<ActionResult<object>> GetROICampañas()
        {
            var campañas = await _context.Campañas
                .Select(c => new
                {
                    c.Id,
                    c.Nombre,
                    c.Presupuesto,
                    c.VentasGeneradas,
                    c.ROI,
                    c.Estado
                })
                .ToListAsync();

            var promedioROI = campañas.Any() ? campañas.Average(c => c.ROI) : 0;

            return Ok(new
            {
                Campañas = campañas,
                PromedioROI = promedioROI,
                FechaConsulta = DateTime.UtcNow
            });
        }

        [HttpGet("performance-mensual")]
        public async Task<ActionResult<object>> GetPerformanceMensual()
        {
            var mesActual = DateTime.UtcNow.Month;
            var añoActual = DateTime.UtcNow.Year;

            var leadsMes = await _context.Leads
                .Where(l => l.FechaCreacion.Month == mesActual && l.FechaCreacion.Year == añoActual)
                .CountAsync();

            var campañasActivas = await _context.Campañas
                .Where(c => c.Estado == "Activa")
                .CountAsync();

            return Ok(new
            {
                Mes = $"{mesActual}/{añoActual}",
                LeadsGenerados = leadsMes,
                CampañasActivas = campañasActivas,
                FechaActualizacion = DateTime.UtcNow
            });
        }

        [HttpGet("leads-calificados")]
        public async Task<ActionResult<object>> GetLeadsCalificados()
        {
            var leadsCalificados = await _context.Leads
                .Where(l => l.Estado == "Calificado" || l.Estado == "Convertido")
                .Include(l => l.Campaña)
                .Select(l => new
                {
                    l.Id,
                    l.Nombre,
                    l.Email,
                    l.Telefono,
                    l.Empresa,
                    Campaña = l.Campaña.Nombre,
                    l.FechaCreacion
                })
                .ToListAsync();

            return Ok(new
            {
                TotalLeadsCalificados = leadsCalificados.Count,
                Leads = leadsCalificados
            });
        }

        [HttpGet("dashboard")]
        public async Task<ActionResult<object>> GetDashboard()
        {
            var totalLeads = await _context.Leads.CountAsync();
            var campañasActivas = await _context.Campañas.CountAsync(c => c.Estado == "Activa");
            var presupuestoTotal = await _context.Campañas.Where(c => c.Estado == "Activa").SumAsync(c => c.Presupuesto);
            var leadsEsteMes = await _context.Leads
                .Where(l => l.FechaCreacion.Month == DateTime.UtcNow.Month && l.FechaCreacion.Year == DateTime.UtcNow.Year)
                .CountAsync();

            return Ok(new
            {
                Metricas = new
                {
                    TotalLeads = totalLeads,
                    CampañasActivas = campañasActivas,
                    PresupuestoTotal = presupuestoTotal,
                    LeadsEsteMes = leadsEsteMes
                },
                UltimosLeads = await _context.Leads
                    .OrderByDescending(l => l.FechaCreacion)
                    .Take(5)
                    .Select(l => new { l.Nombre, l.Email, l.Estado, l.FechaCreacion })
                    .ToListAsync(),
                ProximasCampañas = await _context.Campañas
                    .Where(c => c.FechaInicio > DateTime.UtcNow)
                    .OrderBy(c => c.FechaInicio)
                    .Take(3)
                    .Select(c => new { c.Nombre, c.FechaInicio, c.Presupuesto })
                    .ToListAsync()
            });
        }
    }
}