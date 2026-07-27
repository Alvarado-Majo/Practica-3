using Microsoft.AspNetCore.Mvc;
using VotacionNacional.BLL.DTOs;
using VotacionNacional.BLL.Interfaces;


[ApiController]
        [Route("api/[controller]")]
        public class PartidoController : ControllerBase
        {
            private readonly IPartidoService _partidoService;

            public PartidoController(IPartidoService partidoService)
            {
                _partidoService = partidoService;
            }

            // GET: api/Partido
            [HttpGet]
            public async Task<IActionResult> GetAll()
            {
                var partidos = await _partidoService.GetAllPartidosAsync();
                return Ok(partidos);
            }

            // GET: api/Partido/5
            [HttpGet("{id}")]
            public async Task<IActionResult> GetById(int id)
            {
                var partido = await _partidoService.GetPartidoByIdAsync(id);
                if (partido == null)
                {
                    return NotFound(new { mensaje = "Partido no encontrado" });
                }
                return Ok(partido);
            }

            // GET: api/Partido/nombre/{nombre}
            [HttpGet("nombre/{nombre}")]
            public async Task<IActionResult> GetByNombre(string nombre)
            {
                var partido = await _partidoService.GetPartidoByNombreAsync(nombre);
                if (partido == null)
                {
                    return NotFound(new { mensaje = "Partido no encontrado" });
                }
                return Ok(partido);
            }

            // GET: api/Partido/siglas/{siglas}
            [HttpGet("siglas/{siglas}")]
            public async Task<IActionResult> GetBySiglas(string siglas)
            {
                var partido = await _partidoService.GetPartidoBySiglasAsync(siglas);
                if (partido == null)
                {
                    return NotFound(new { mensaje = "Partido no encontrado" });
                }
                return Ok(partido);
            }

            // POST: api/Partido
            [HttpPost]
            public async Task<IActionResult> Create([FromBody] CrearPartidoDTO partidoDto)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var resultado = await _partidoService.CreatePartidoAsync(partidoDto);

                if (!resultado.Exitoso)
                {
                    return BadRequest(resultado);
                }

                return Ok(resultado);
            }

            // PUT: api/Partido/5
            [HttpPut("{id}")]
            public async Task<IActionResult> Update(int id, [FromBody] ActualizarPartidoDTO partidoDto)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var resultado = await _partidoService.UpdatePartidoAsync(id, partidoDto);

                if (!resultado.Exitoso)
                {
                    return BadRequest(resultado);
                }

                return Ok(resultado);
            }

            // DELETE: api/Partido/5
            [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(int id)
            {
                var resultado = await _partidoService.DeletePartidoByIdAsync(id);

                if (!resultado)
                {
                    return NotFound(new { mensaje = "No se pudo eliminar el partido, puede que no exista" });
                }

                return Ok(new { mensaje = "Partido eliminado correctamente" });
            }
        }
