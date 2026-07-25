using VotacionNacional.BLL.DTOs;

namespace VotacionNacional.BLL.Interfaces
{
    public interface IVotanteService
    {
        Task<List<VotanteDto>> ObtenerTodosAsync();
        Task<VotanteDto?> ObtenerPorIdAsync(int id);
        Task<VotanteDto?> ObtenerPorCedulaAsync(string cedula);

        Task<(ResultadoOperacionDto Resultado, VotanteDto? Votante)>
            AgregarAsync(CrearVotanteDto dto);

        Task<ResultadoOperacionDto> ActualizarAsync(
            int id,
            ActualizarVotanteDto dto);

        Task<ResultadoOperacionDto> EliminarAsync(int id);
    }
}
