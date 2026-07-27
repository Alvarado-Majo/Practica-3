using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;
using VotacionNacional.BLL.DTOs;

namespace VotacionNacional.Controllers
{
    public class PartidoController : Controller
    {
        private readonly HttpClient _httpClient;

        public PartidoController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("VotacionAPI");
        }

        // GET: /Partido
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var partidos =
                    await _httpClient
                        .GetFromJsonAsync<List<MostrarPartidoDTO>>(
                            "api/Partidos"
                        );

                return View(
                    partidos ?? new List<MostrarPartidoDTO>()
                );
            }
            catch (HttpRequestException)
            {
                TempData["Error"] =
                    "No fue posible comunicarse con la API.";

                return View(new List<MostrarPartidoDTO>());
            }
            catch (Exception)
            {
                TempData["Error"] =
                    "Ocurrió un error al cargar los partidos.";

                return View(new List<MostrarPartidoDTO>());
            }
        }

        // GET: /Partido/Crear
        [HttpGet]
        public IActionResult Crear()
        {
            return View(new CrearPartidoDTO());
        }

        // POST: /Partido/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(CrearPartidoDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                HttpResponseMessage respuesta =
                    await _httpClient.PostAsJsonAsync(
                        "api/Partidos",
                        dto
                    );

                if (respuesta.IsSuccessStatusCode)
                {
                    TempData["Exito"] =
                        "El partido fue registrado correctamente.";

                    return RedirectToAction(nameof(Index));
                }

                ResultadoOperacionDto? resultado =
                    await LeerResultadoAsync(respuesta);

                ModelState.AddModelError(
                    string.Empty,
                    resultado?.Mensaje ??
                    "No fue posible registrar el partido."
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
                    "Ocurrió un error al registrar el partido."
                );
            }

            return View(dto);
        }

        // GET: /Partido/Editar/5
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            try
            {
                HttpResponseMessage respuesta =
                    await _httpClient.GetAsync(
                        $"api/Partidos/{id}"
                    );

                if (respuesta.StatusCode == HttpStatusCode.NotFound)
                {
                    TempData["Error"] =
                        "No se encontró el partido.";

                    return RedirectToAction(nameof(Index));
                }

                if (!respuesta.IsSuccessStatusCode)
                {
                    TempData["Error"] =
                        "No fue posible obtener el partido.";

                    return RedirectToAction(nameof(Index));
                }

                MostrarPartidoDTO? partido =
                    await respuesta.Content
                        .ReadFromJsonAsync<MostrarPartidoDTO>();

                if (partido is null)
                {
                    TempData["Error"] =
                        "La API no devolvió los datos del partido.";

                    return RedirectToAction(nameof(Index));
                }

                var dto = new ActualizarPartidoDTO
                {
                    Nombre = partido.Nombre,
                    Siglas = partido.Siglas,
                    ImagenUrl = partido.ImagenUrl,
                    Activo = partido.Activo
                };

                ViewBag.PartidoId = id;

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
                    "Ocurrió un error al cargar el partido.";

                return RedirectToAction(nameof(Index));
            }
        }

        // POST: /Partido/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(
            int id,
            ActualizarPartidoDTO dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PartidoId = id;
                return View(dto);
            }

            try
            {
                HttpResponseMessage respuesta =
                    await _httpClient.PutAsJsonAsync(
                        $"api/Partidos/{id}",
                        dto
                    );

                if (respuesta.IsSuccessStatusCode)
                {
                    TempData["Exito"] =
                        "El partido fue actualizado correctamente.";

                    return RedirectToAction(nameof(Index));
                }

                ResultadoOperacionDto? resultado =
                    await LeerResultadoAsync(respuesta);

                ModelState.AddModelError(
                    string.Empty,
                    resultado?.Mensaje ??
                    "No fue posible actualizar el partido."
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
                    "Ocurrió un error al actualizar el partido."
                );
            }

            ViewBag.PartidoId = id;

            return View(dto);
        }

        // GET: /Partido/Eliminar/5
        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                HttpResponseMessage respuesta =
                    await _httpClient.GetAsync(
                        $"api/Partidos/{id}"
                    );

                if (respuesta.StatusCode == HttpStatusCode.NotFound)
                {
                    TempData["Error"] =
                        "No se encontró el partido.";

                    return RedirectToAction(nameof(Index));
                }

                if (!respuesta.IsSuccessStatusCode)
                {
                    TempData["Error"] =
                        "No fue posible obtener el partido.";

                    return RedirectToAction(nameof(Index));
                }

                MostrarPartidoDTO? partido =
                    await respuesta.Content
                        .ReadFromJsonAsync<MostrarPartidoDTO>();

                if (partido is null)
                {
                    TempData["Error"] =
                        "La API no devolvió los datos del partido.";

                    return RedirectToAction(nameof(Index));
                }

                return View(partido);
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
                    "Ocurrió un error al cargar el partido.";

                return RedirectToAction(nameof(Index));
            }
        }

        // POST: /Partido/EliminarConfirmado/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            try
            {
                HttpResponseMessage respuesta =
                    await _httpClient.DeleteAsync(
                        $"api/Partidos/{id}"
                    );

                if (respuesta.IsSuccessStatusCode)
                {
                    TempData["Exito"] =
                        "El partido fue eliminado correctamente.";
                }
                else
                {
                    ResultadoOperacionDto? resultado =
                        await LeerResultadoAsync(respuesta);

                    TempData["Error"] =
                        resultado?.Mensaje ??
                        "No fue posible eliminar el partido.";
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
                    "Ocurrió un error al eliminar el partido.";
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