using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using VotacionNacional.BLL.DTOs;

namespace VotacionNacional.Controllers
{
    public class VotacionController : Controller
    {
        private readonly HttpClient _httpClient;

        public VotacionController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("VotacionAPI");
        }

        // GET: /Votacion
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // POST: /Votacion/ValidarCedula
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ValidarCedula(string cedula)
        {
            if (string.IsNullOrWhiteSpace(cedula))
            {
                TempData["Error"] = "Debe ingresar una cédula.";

                return RedirectToAction(nameof(Index));
            }

            try
            {
                VotanteDto? votante =
                    await _httpClient.GetFromJsonAsync<VotanteDto>(
                        $"api/Votantes/cedula/{cedula.Trim()}"
                    );

                if (votante is null)
                {
                    TempData["Error"] =
                        "La persona no está inscrita como votante.";

                    return RedirectToAction(nameof(Index));
                }

                return RedirectToAction(
                    nameof(Votar),
                    new { cedula = votante.Cedula }
                );
            }
            catch
            {
                TempData["Error"] =
                    "No fue posible validar la cédula.";

                return RedirectToAction(nameof(Index));
            }
        }

        // GET: /Votacion/Votar
        [HttpGet]
        public async Task<IActionResult> Votar(string cedula)
        {
            if (string.IsNullOrWhiteSpace(cedula))
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                VotanteDto? votante =
                    await _httpClient.GetFromJsonAsync<VotanteDto>(
                        $"api/Votantes/cedula/{cedula}"
                    );

                if (votante is null)
                {
                    TempData["Error"] =
                        "La persona no está inscrita como votante.";

                    return RedirectToAction(nameof(Index));
                }

                List<MostrarPartidoDTO>? partidos =
                    await _httpClient
                        .GetFromJsonAsync<List<MostrarPartidoDTO>>(
                            "api/Partidos"
                        );

                ViewBag.Votante = votante;
                ViewBag.Cedula = cedula;

                return View(
                    partidos ?? new List<MostrarPartidoDTO>()
                );
            }
            catch
            {
                TempData["Error"] =
                    "No fue posible cargar la información para votar.";

                return RedirectToAction(nameof(Index));
            }
        }

        // POST: /Votacion/Votar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Votar(
            RegistrarVotoDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(
                    nameof(Votar),
                    new { cedula = dto.Cedula }
                );
            }

            try
            {
                HttpResponseMessage respuesta =
                    await _httpClient.PostAsJsonAsync(
                        "api/Votacion",
                        dto
                    );

                ResultadoOperacionDto? resultado =
                    await respuesta.Content
                        .ReadFromJsonAsync<ResultadoOperacionDto>();

                if (respuesta.IsSuccessStatusCode)
                {
                    TempData["Exito"] =
                        resultado?.Mensaje ??
                        "Voto registrado correctamente.";
                }
                else
                {
                    TempData["Error"] =
                        resultado?.Mensaje ??
                        "No fue posible registrar el voto.";
                }
            }
            catch
            {
                TempData["Error"] =
                    "No fue posible comunicarse con la API.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}