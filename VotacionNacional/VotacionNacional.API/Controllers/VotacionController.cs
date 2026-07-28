using Microsoft.AspNetCore.Mvc;
using VotacionNacional.BLL.DTOs;
using VotacionNacional.BLL.Interfaces;

namespace VotacionNacional.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VotacionController : ControllerBase
    {
        private readonly IVotacionService _votacionService;

        public VotacionController(
            IVotacionService votacionService)
        {
            _votacionService = votacionService;
        }

        // POST: api/Votacion
        [HttpPost]
        public async Task<IActionResult> RegistrarVoto(
            [FromBody] RegistrarVotoDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            ResultadoOperacionDto resultado =
                await _votacionService
                    .RegistrarVotoAsync(dto);

            if (!resultado.Exitoso)
            {
                return BadRequest(resultado);
            }

            return Ok(resultado);
        }
    }
}