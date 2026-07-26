using System;
using System.Collections.Generic;
using System.Text;
using VotacionNacional.DAL.Entities;

namespace VotacionNacional.DAL.Interfaces
{
    public interface IPartidoRepository
    {
        Task<List<Partido>> ObtenerPartidosAsync();
        Task<Partido?> ObtenerPartidosPorIdAsync(int id);
        Task<Partido?> ObtenerPartidosPorSiglasAsync(string siglas);
        Task<Partido?> ObtenerPartidosPorNombreAsync(string nombre);
        Task<bool> AgregarPartidoAsync(Partido partido);
        Task<bool> ActualizarPartidoAsync(Partido partido);
        Task<bool> EliminarPartidoAsync(int id);
        Task<int> ContarVotosPorPartidoAsync(int partidoId);
    }
}
