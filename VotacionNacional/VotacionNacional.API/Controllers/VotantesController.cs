using Microsoft.AspNetCore.Mvc;
using VotacionNacional.BLL.DTOs;
using VotacionNacional.BLL.Interfaces;

namespace VotacionNacional.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotantesController : ControllerBase
    {
        private readonly IVotanteService _votanteService;

        public VotantesController(IVotanteService votanteService)
        {
            _votanteService = votanteService;
        }

        [HttpGet]
        public async Task<ActionResult<List<VotanteDto>>> ObtenerTodos()
        {
            List<VotanteDto> votantes =
                await _votanteService.ObtenerTodosAsync();

            return Ok(votantes);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<VotanteDto>> ObtenerPorId(int id)
        {
            VotanteDto? votante =
                await _votanteService.ObtenerPorIdAsync(id);

            if (votante is null)
            {
                return NotFound(new
                {
                    mensaje = "No se encontró el votante."
                });
            }

            return Ok(votante);
        }

        [HttpGet("cedula/{cedula}")]
        public async Task<ActionResult<VotanteDto>> ObtenerPorCedula(
            string cedula)
        {
            VotanteDto? votante =
                await _votanteService.ObtenerPorCedulaAsync(cedula);

            if (votante is null)
            {
                return NotFound(new
                {
                    mensaje = "La persona no está inscrita como votante."
                });
            }

            return Ok(votante);
        }

        [HttpPost]
        public async Task<ActionResult<VotanteDto>> Agregar(
            [FromBody] CrearVotanteDto dto)
        {
            var respuesta = await _votanteService.AgregarAsync(dto);

            if (!respuesta.Resultado.Exitoso)
            {
                return BadRequest(new
                {
                    mensaje = respuesta.Resultado.Mensaje
                });
            }

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = respuesta.Votante!.VotanteId },
                respuesta.Votante);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(
            int id,
            [FromBody] ActualizarVotanteDto dto)
        {
            ResultadoOperacionDto resultado =
                await _votanteService.ActualizarAsync(id, dto);

            if (!resultado.Exitoso)
            {
                if (resultado.Mensaje == "No se encontró el votante.")
                {
                    return NotFound(new
                    {
                        mensaje = resultado.Mensaje
                    });
                }

                return BadRequest(new
                {
                    mensaje = resultado.Mensaje
                });
            }

            return Ok(new
            {
                mensaje = resultado.Mensaje
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            ResultadoOperacionDto resultado =
                await _votanteService.EliminarAsync(id);

            if (!resultado.Exitoso)
            {
                return NotFound(new
                {
                    mensaje = resultado.Mensaje
                });
            }

            return Ok(new
            {
                mensaje = resultado.Mensaje
            });
        }
    }
}
