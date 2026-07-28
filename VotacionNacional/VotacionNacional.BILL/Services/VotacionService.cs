using VotacionNacional.BLL.DTOs;
using VotacionNacional.BLL.Interfaces;
using VotacionNacional.DAL.Entities;
using VotacionNacional.DAL.Interfaces;

namespace VotacionNacional.BLL.Services
{
    public class VotacionService : IVotacionService
    {
        private readonly IVotanteRepository _votanteRepository;
        private readonly IPartidoRepository _partidoRepository;
        private readonly IVotoRepository _votoRepository;

        public VotacionService(
            IVotanteRepository votanteRepository,
            IPartidoRepository partidoRepository,
            IVotoRepository votoRepository)
        {
            _votanteRepository = votanteRepository;
            _partidoRepository = partidoRepository;
            _votoRepository = votoRepository;
        }

        public async Task<ResultadoOperacionDto> RegistrarVotoAsync(
            RegistrarVotoDTO dto)
        {
            var votante =
                await _votanteRepository
                    .ObtenerPorCedulaAsync(dto.Cedula.Trim());

            if (votante is null)
            {
                return new ResultadoOperacionDto
                {
                    Exitoso = false,
                    Mensaje = "La persona no está inscrita como votante."
                };
            }

            if (!votante.Activo)
            {
                return new ResultadoOperacionDto
                {
                    Exitoso = false,
                    Mensaje = "El votante no se encuentra activo."
                };
            }

            var partido =
                await _partidoRepository
                    .ObtenerPartidosPorIdAsync(dto.PartidoId);

            if (partido is null)
            {
                return new ResultadoOperacionDto
                {
                    Exitoso = false,
                    Mensaje = "El partido seleccionado no existe."
                };
            }

            if (!partido.Activo)
            {
                return new ResultadoOperacionDto
                {
                    Exitoso = false,
                    Mensaje = "El partido seleccionado no está activo."
                };
            }

            bool yaVoto =
                await _votoRepository
                    .VotanteYaVotoAsync(votante.VotanteId);

            if (yaVoto)
            {
                return new ResultadoOperacionDto
                {
                    Exitoso = false,
                    Mensaje = "El votante ya registró un voto."
                };
            }

            Voto voto = new()
            {
                VotanteId = votante.VotanteId,
                PartidoId = partido.PartidoId
            };

            await _votoRepository.RegistrarVotoAsync(voto);

            return new ResultadoOperacionDto
            {
                Exitoso = true,
                Mensaje = "Voto registrado correctamente."
            };
        }
    }
}