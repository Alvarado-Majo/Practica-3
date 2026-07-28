using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using VotacionNacional.BLL.DTOs;

namespace VotacionNacional.Controllers
{
    public class ResultadosController : Controller
    {
        private readonly HttpClient _httpClient;

        public ResultadosController(
            IHttpClientFactory httpClientFactory)
        {
            _httpClient =
                httpClientFactory.CreateClient("VotacionAPI");
        }

        public async Task<IActionResult> Index()
        {
            var partidos =
                await _httpClient.GetFromJsonAsync<
                    List<MostrarPartidoDTO>>(
                    "api/Partidos");

            return View(
                partidos ?? new List<MostrarPartidoDTO>());
        }
    }
}