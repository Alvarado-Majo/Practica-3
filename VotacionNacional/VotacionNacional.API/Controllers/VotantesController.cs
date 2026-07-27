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

        // GET: api/Votantes
        [HttpGet]
        public async Task<ActionResult<List<VotanteDto>>> ObtenerTodos()
        {
            List<VotanteDto> votantes =
                await _votanteService.ObtenerTodosAsync();

            return Ok(votantes);
        }

        // GET: api/Votantes/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<VotanteDto>> ObtenerPorId(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    mensaje = "El identificador del votante no es válido."
                });
            }

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

        // GET: api/Votantes/cedula/123456789
        [HttpGet("cedula/{cedula}")]
        public async Task<ActionResult<VotanteDto>> ObtenerPorCedula(
            string cedula)
        {
            if (string.IsNullOrWhiteSpace(cedula))
            {
                return BadRequest(new
                {
                    mensaje = "Debe proporcionar una cédula."
                });
            }

            VotanteDto? votante =
                await _votanteService.ObtenerPorCedulaAsync(
                    cedula.Trim()
                );

            if (votante is null)
            {
                return NotFound(new
                {
                    mensaje = "La persona no está inscrita como votante."
                });
            }

            return Ok(votante);
        }

        // POST: api/Votantes
        [HttpPost]
        public async Task<ActionResult<VotanteDto>> Agregar(
            [FromBody] CrearVotanteDto dto)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var respuesta =
                await _votanteService.AgregarAsync(dto);

            if (!respuesta.Resultado.Exitoso)
            {
                return BadRequest(new
                {
                    mensaje = respuesta.Resultado.Mensaje
                });
            }

            if (respuesta.Votante is null)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        mensaje =
                            "El votante fue procesado, pero no se obtuvo su información."
                    }
                );
            }

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new
                {
                    id = respuesta.Votante.VotanteId
                },
                respuesta.Votante
            );
        }

        // PUT: api/Votantes/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(
            int id,
            [FromBody] ActualizarVotanteDto dto)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    mensaje = "El identificador del votante no es válido."
                });
            }

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

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

        // DELETE: api/Votantes/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    mensaje = "El identificador del votante no es válido."
                });
            }

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