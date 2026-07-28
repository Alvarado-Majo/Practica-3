using System;
using System.Collections.Generic;
using System.Text;
using VotacionNacional.BLL.DTOs;

namespace VotacionNacional.BLL.Interfaces
{
    public interface IVotacionService
    {
        Task<ResultadoOperacionDto> RegistrarVotoAsync(RegistrarVotoDTO dto);
    }
}
