using System;
using System.Collections.Generic;
using System.Text;
using VotacionNacional.BLL.DTOs;

namespace VotacionNacional.BLL.Interfaces
{
    public interface IPartidoService
    {
        Task<List<MostrarPartidoDTO>> GetAllPartidosAsync();
        Task<MostrarPartidoDTO> GetPartidoByIdAsync(int id);
        Task<MostrarPartidoDTO> GetPartidoBySiglasAsync(string siglas);
        Task<MostrarPartidoDTO> GetPartidoByNombreAsync(string nombre);
        Task<ResultadoOperacionDto> CreatePartidoAsync(CrearPartidoDTO partidoDto);
        Task<ResultadoOperacionDto> UpdatePartidoAsync(int id, ActualizarPartidoDTO partidoDto);
        Task<bool> DeletePartidoByIdAsync(int id);
    }
}
