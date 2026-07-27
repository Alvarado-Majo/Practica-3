using Microsoft.AspNetCore.Mvc;
using VotacionNacional.BLL.DTOs;
using VotacionNacional.BLL.Interfaces;

namespace VotacionNacional.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartidosController : ControllerBase
    {
        private readonly IPartidoService _partidoService;

        public PartidosController(IPartidoService partidoService)
        {
            _partidoService = partidoService;
        }

        // GET: api/Partidos
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            try
            {
                var partidos =
                    await _partidoService.GetAllPartidosAsync();

                return Ok(partidos);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        mensaje = "Ocurrió un error al obtener los partidos.",
                        detalle = ex.Message
                    }
                );
            }
        }

        // GET: api/Partidos/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        mensaje = "El identificador del partido no es válido."
                    });
                }

                var partido =
                    await _partidoService.GetPartidoByIdAsync(id);

                if (partido is null)
                {
                    return NotFound(new
                    {
                        mensaje = "No se encontró el partido."
                    });
                }

                return Ok(partido);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        mensaje = "Ocurrió un error al obtener el partido.",
                        detalle = ex.Message
                    }
                );
            }
        }

        // GET: api/Partidos/nombre/Liberacion
        [HttpGet("nombre/{nombre}")]
        public async Task<IActionResult> ObtenerPorNombre(string nombre)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombre))
                {
                    return BadRequest(new
                    {
                        mensaje = "Debe proporcionar el nombre del partido."
                    });
                }

                var partido =
                    await _partidoService.GetPartidoByNombreAsync(
                        nombre.Trim()
                    );

                if (partido is null)
                {
                    return NotFound(new
                    {
                        mensaje =
                            "No se encontró un partido con ese nombre."
                    });
                }

                return Ok(partido);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        mensaje = "Ocurrió un error al buscar el partido.",
                        detalle = ex.Message
                    }
                );
            }
        }

        // GET: api/Partidos/siglas/PLN
        [HttpGet("siglas/{siglas}")]
        public async Task<IActionResult> ObtenerPorSiglas(string siglas)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(siglas))
                {
                    return BadRequest(new
                    {
                        mensaje = "Debe proporcionar las siglas del partido."
                    });
                }

                var partido =
                    await _partidoService.GetPartidoBySiglasAsync(
                        siglas.Trim()
                    );

                if (partido is null)
                {
                    return NotFound(new
                    {
                        mensaje =
                            "No se encontró un partido con esas siglas."
                    });
                }

                return Ok(partido);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        mensaje = "Ocurrió un error al buscar el partido.",
                        detalle = ex.Message
                    }
                );
            }
        }

        // POST: api/Partidos
        [HttpPost]
        public async Task<IActionResult> Crear(
            [FromBody] CrearPartidoDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationProblem(ModelState);
                }

                var resultado =
                    await _partidoService.CreatePartidoAsync(dto);

                if (!resultado.Exitoso)
                {
                    return BadRequest(resultado);
                }

                return StatusCode(
                    StatusCodes.Status201Created,
                    resultado
                );
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        mensaje = "Ocurrió un error al registrar el partido.",
                        detalle = ex.Message
                    }
                );
            }
        }

        // PUT: api/Partidos/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(
            int id,
            [FromBody] ActualizarPartidoDTO dto)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        mensaje = "El identificador del partido no es válido."
                    });
                }

                if (!ModelState.IsValid)
                {
                    return ValidationProblem(ModelState);
                }

                var resultado =
                    await _partidoService.UpdatePartidoAsync(id, dto);

                if (!resultado.Exitoso)
                {
                    return BadRequest(resultado);
                }

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        mensaje = "Ocurrió un error al actualizar el partido.",
                        detalle = ex.Message
                    }
                );
            }
        }

        // DELETE: api/Partidos/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        mensaje = "El identificador del partido no es válido."
                    });
                }

                var eliminado =
                    await _partidoService.DeletePartidoByIdAsync(id);

                if (!eliminado)
                {
                    return NotFound(new
                    {
                        mensaje =
                            "No se pudo eliminar el partido. Puede que no exista."
                    });
                }

                return Ok(new ResultadoOperacionDto
                {
                    Exitoso = true,
                    Mensaje = "El partido fue eliminado correctamente."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        mensaje = "Ocurrió un error al eliminar el partido.",
                        detalle = ex.Message
                    }
                );
            }
        }
    }
}