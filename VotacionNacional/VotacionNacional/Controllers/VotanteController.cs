using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;
using VotacionNacional.BLL.DTOs;

namespace VotacionNacional.Controllers
{
    public class VotanteController : Controller
    {
        private readonly HttpClient _httpClient;

        public VotanteController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("VotacionAPI");
        }

        // GET: /Votante
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                HttpResponseMessage respuesta =
                    await _httpClient.GetAsync("api/Votantes");

                if (!respuesta.IsSuccessStatusCode)
                {
                    string detalle =
                        await respuesta.Content.ReadAsStringAsync();

                    TempData["Error"] =
                        $"La API respondió {(int)respuesta.StatusCode} " +
                        $"{respuesta.StatusCode}. Detalle: {detalle}";

                    return View(new List<VotanteDto>());
                }

                var votantes =
                    await respuesta.Content
                        .ReadFromJsonAsync<List<VotanteDto>>();

                return View(
                    votantes ?? new List<VotanteDto>()
                );
            }
            catch (Exception ex)
            {
                TempData["Error"] =
                    $"{ex.GetType().Name}: {ex.Message}";

                return View(new List<VotanteDto>());
            }
        }

        // GET: /Votante/Crear
        [HttpGet]
        public IActionResult Crear()
        {
            return View(new CrearVotanteDto());
        }

        // POST: /Votante/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(CrearVotanteDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                HttpResponseMessage respuesta =
                    await _httpClient.PostAsJsonAsync(
                        "api/Votantes",
                        dto
                    );

                if (respuesta.IsSuccessStatusCode)
                {
                    TempData["Exito"] =
                        "El votante fue registrado correctamente.";

                    return RedirectToAction(nameof(Index));
                }

                ResultadoOperacionDto? resultado =
                    await LeerResultadoAsync(respuesta);

                ModelState.AddModelError(
                    string.Empty,
                    resultado?.Mensaje ??
                    "No fue posible registrar el votante."
                );
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "No fue posible comunicarse con la API."
                );
            }
            catch (Exception)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Ocurrió un error al registrar el votante."
                );
            }

            return View(dto);
        }

        // GET: /Votante/Editar/5
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            try
            {
                HttpResponseMessage respuesta =
                    await _httpClient.GetAsync(
                        $"api/Votantes/{id}"
                    );

                if (respuesta.StatusCode == HttpStatusCode.NotFound)
                {
                    TempData["Error"] =
                        "No se encontró el votante.";

                    return RedirectToAction(nameof(Index));
                }

                if (!respuesta.IsSuccessStatusCode)
                {
                    TempData["Error"] =
                        "No fue posible obtener el votante.";

                    return RedirectToAction(nameof(Index));
                }

                VotanteDto? votante =
                    await respuesta.Content
                        .ReadFromJsonAsync<VotanteDto>();

                if (votante is null)
                {
                    TempData["Error"] =
                        "La API no devolvió los datos del votante.";

                    return RedirectToAction(nameof(Index));
                }

                var dto = new ActualizarVotanteDto
                {
                    Cedula = votante.Cedula,
                    Nombre = votante.Nombre,
                    PrimerApellido = votante.PrimerApellido,
                    SegundoApellido = votante.SegundoApellido,
                    FechaNacimiento = votante.FechaNacimiento,
                    Activo = votante.Activo
                };

                ViewBag.VotanteId = id;

                return View(dto);
            }
            catch (HttpRequestException)
            {
                TempData["Error"] =
                    "No fue posible comunicarse con la API.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["Error"] =
                    "Ocurrió un error al cargar el votante.";

                return RedirectToAction(nameof(Index));
            }
        }

        // POST: /Votante/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(
            int id,
            ActualizarVotanteDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.VotanteId = id;
                return View(dto);
            }

            try
            {
                HttpResponseMessage respuesta =
                    await _httpClient.PutAsJsonAsync(
                        $"api/Votantes/{id}",
                        dto
                    );

                if (respuesta.IsSuccessStatusCode)
                {
                    TempData["Exito"] =
                        "El votante fue actualizado correctamente.";

                    return RedirectToAction(nameof(Index));
                }

                ResultadoOperacionDto? resultado =
                    await LeerResultadoAsync(respuesta);

                ModelState.AddModelError(
                    string.Empty,
                    resultado?.Mensaje ??
                    "No fue posible actualizar el votante."
                );
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "No fue posible comunicarse con la API."
                );
            }
            catch (Exception)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Ocurrió un error al actualizar el votante."
                );
            }

            ViewBag.VotanteId = id;

            return View(dto);
        }

        // GET: /Votante/Eliminar/5
        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                HttpResponseMessage respuesta =
                    await _httpClient.GetAsync(
                        $"api/Votantes/{id}"
                    );

                if (respuesta.StatusCode == HttpStatusCode.NotFound)
                {
                    TempData["Error"] =
                        "No se encontró el votante.";

                    return RedirectToAction(nameof(Index));
                }

                if (!respuesta.IsSuccessStatusCode)
                {
                    TempData["Error"] =
                        "No fue posible obtener el votante.";

                    return RedirectToAction(nameof(Index));
                }

                VotanteDto? votante =
                    await respuesta.Content
                        .ReadFromJsonAsync<VotanteDto>();

                if (votante is null)
                {
                    TempData["Error"] =
                        "La API no devolvió los datos del votante.";

                    return RedirectToAction(nameof(Index));
                }

                return View(votante);
            }
            catch (HttpRequestException)
            {
                TempData["Error"] =
                    "No fue posible comunicarse con la API.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["Error"] =
                    "Ocurrió un error al cargar el votante.";

                return RedirectToAction(nameof(Index));
            }
        }

        // POST: /Votante/EliminarConfirmado/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            try
            {
                HttpResponseMessage respuesta =
                    await _httpClient.DeleteAsync(
                        $"api/Votantes/{id}"
                    );

                if (respuesta.IsSuccessStatusCode)
                {
                    TempData["Exito"] =
                        "El votante fue eliminado correctamente.";
                }
                else
                {
                    ResultadoOperacionDto? resultado =
                        await LeerResultadoAsync(respuesta);

                    TempData["Error"] =
                        resultado?.Mensaje ??
                        "No fue posible eliminar el votante.";
                }
            }
            catch (HttpRequestException)
            {
                TempData["Error"] =
                    "No fue posible comunicarse con la API.";
            }
            catch (Exception)
            {
                TempData["Error"] =
                    "Ocurrió un error al eliminar el votante.";
            }

            return RedirectToAction(nameof(Index));
        }

        private static async Task<ResultadoOperacionDto?>
            LeerResultadoAsync(HttpResponseMessage respuesta)
        {
            try
            {
                return await respuesta.Content
                    .ReadFromJsonAsync<ResultadoOperacionDto>();
            }
            catch
            {
                return null;
            }
        }
    }
}