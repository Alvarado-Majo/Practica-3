using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VotacionNacional.BLL.DTOs;
using VotacionNacional.BLL.Interfaces;

namespace VotacionNacional.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VotanteController : ControllerBase
    {
        private readonly IVotanteService _votanteService;

        public VotanteController(IVotanteService votanteService)
        {
            _votanteService = votanteService;
        }

        // GET: api/Votante
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var votantes = await _votanteService.ObtenerTodosAsync();
            return Ok(votantes);
        }

        // GET: api/Votante/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var votante = await _votanteService.ObtenerPorIdAsync(id);
            if (votante is null)
            {
                return NotFound(new { mensaje = "No se encontró el votante." });
            }
            return Ok(votante);
        }

        // GET: api/Votante/cedula/{cedula}
        [HttpGet("cedula/{cedula}")]
        public async Task<IActionResult> GetByCedula(string cedula)
        {
            var votante = await _votanteService.ObtenerPorCedulaAsync(cedula);
            if (votante is null)
            {
                return NotFound(new { mensaje = "No se encontró el votante." });
            }
            return Ok(votante);
        }

        // POST: api/Votante
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CrearVotanteDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (resultado, votante) = await _votanteService.AgregarAsync(dto);

            if (!resultado.Exitoso)
            {
                return BadRequest(resultado);
            }

            return CreatedAtAction(nameof(GetById), new { id = votante!.VotanteId }, resultado);
        }

        // PUT: api/Votante/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ActualizarVotanteDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resultado = await _votanteService.ActualizarAsync(id, dto);

            if (!resultado.Exitoso)
            {
                return BadRequest(resultado);
            }

            return Ok(resultado);
        }

        // DELETE: api/Votante/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var resultado = await _votanteService.EliminarAsync(id);

            if (!resultado.Exitoso)
            {
                return NotFound(resultado);
            }

            return Ok(resultado);
        }
    }
}
