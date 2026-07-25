using VotacionNacional.DAL.Entities;

namespace VotacionNacional.DAL.Interfaces
{
    public interface IVotanteRepository
    {
        Task<List<Votante>> ObtenerTodosAsync();
        Task<Votante?> ObtenerPorIdAsync(int id);
        Task<Votante?> ObtenerPorCedulaAsync(string cedula);
        Task<bool> ExisteCedulaAsync(string cedula, int? excluirId = null);
        Task<Votante> AgregarAsync(Votante votante);
        Task<bool> ActualizarAsync(Votante votante);
        Task<bool> EliminarAsync(int id);
    }
}
