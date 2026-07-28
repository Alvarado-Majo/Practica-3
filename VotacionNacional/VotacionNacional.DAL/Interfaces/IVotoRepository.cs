using System;
using System.Collections.Generic;
using System.Text;
using VotacionNacional.DAL.Entities;

namespace VotacionNacional.DAL.Interfaces
{
    public interface IVotoRepository
    {
        Task<Voto> RegistrarVotoAsync(Voto voto);

        Task<bool> VotanteYaVotoAsync(int votanteId);

        Task<List<Voto>> ObtenerTodosAsync();
    }
}