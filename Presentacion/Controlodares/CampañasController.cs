using Microsoft.AspNetCore.Mvc;
using MarketingLaPazAPI.Core.Interfaces;
using MarketingLaPazAPI.Core.DTOs;

namespace MarketingLaPazAPI.Presentacion.Controladores
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampañasController : ControllerBase
    {
        private readonly IServicioCampaña _servicioCampaña;

        public CampañasController(IServicioCampaña servicioCampaña)
        {
            _servicioCampaña = servicioCampaña;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampañaDTO>>> GetCampañas()
        {
            var campañas = await _servicioCampaña.ObtenerTodasLasCampañasAsync();
            return Ok(campañas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CampañaDTO>> GetCampaña(int id)
        {
            var campaña = await _servicioCampaña.ObtenerCampañaPorIdAsync(id);
            if (campaña == null)
            {
                return NotFound();
            }
            return campaña;
        }

        [HttpPost]
        public async Task<ActionResult<CampañaDTO>> PostCampaña(CrearCampañaDTO campañaDTO)
        {
            var campañaCreada = await _servicioCampaña.CrearCampañaAsync(campañaDTO);
            return CreatedAtAction("GetCampaña", new { id = campañaCreada.Id }, campañaCreada);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCampaña(int id, ActualizarCampañaDTO campañaDTO)
        {
            var campañaActualizada = await _servicioCampaña.ActualizarCampañaAsync(id, campañaDTO);
            if (campañaActualizada == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampaña(int id)
        {
            var resultado = await _servicioCampaña.EliminarCampañaAsync(id);
            if (!resultado)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("activas")]
        public async Task<ActionResult<IEnumerable<CampañaDTO>>> GetCampañasActivas()
        {
            var campañas = await _servicioCampaña.ObtenerCampañasActivasAsync();
            return Ok(campañas);
        }

        [HttpGet("estadisticas")]
        public async Task<ActionResult<object>> GetEstadisticas()
        {
            var estadisticas = await _servicioCampaña.ObtenerEstadisticasCampañasAsync();
            return Ok(estadisticas);
        }
    }
}