using VotacionNacional.BLL.DTOs;
using VotacionNacional.BLL.Interfaces;
using VotacionNacional.DAL.Entities;
using VotacionNacional.DAL.Interfaces;

namespace VotacionNacional.BLL.Services
{
    public class VotanteService : IVotanteService
    {
        private readonly IVotanteRepository _votanteRepository;

        public VotanteService(IVotanteRepository votanteRepository)
        {
            _votanteRepository = votanteRepository;
        }

        public async Task<List<VotanteDto>> ObtenerTodosAsync()
        {
            List<Votante> votantes =
                await _votanteRepository.ObtenerTodosAsync();

            return votantes.Select(ConvertirADto).ToList();
        }

        public async Task<VotanteDto?> ObtenerPorIdAsync(int id)
        {
            Votante? votante =
                await _votanteRepository.ObtenerPorIdAsync(id);

            return votante is null ? null : ConvertirADto(votante);
        }

        public async Task<VotanteDto?> ObtenerPorCedulaAsync(string cedula)
        {
            Votante? votante =
                await _votanteRepository.ObtenerPorCedulaAsync(cedula);

            return votante is null ? null : ConvertirADto(votante);
        }

        public async Task<(ResultadoOperacionDto Resultado, VotanteDto? Votante)>
            AgregarAsync(CrearVotanteDto dto)
        {
            if (await _votanteRepository.ExisteCedulaAsync(dto.Cedula))
            {
                return (
                    new ResultadoOperacionDto
                    {
                        Exitoso = false,
                        Mensaje = "Ya existe un votante con esa cédula."
                    },
                    null
                );
            }

            if (dto.FechaNacimiento.Date > DateTime.Today)
            {
                return (
                    new ResultadoOperacionDto
                    {
                        Exitoso = false,
                        Mensaje = "La fecha de nacimiento no puede ser futura."
                    },
                    null
                );
            }

            Votante nuevoVotante = new()
            {
                Cedula = dto.Cedula.Trim(),
                Nombre = dto.Nombre.Trim(),
                PrimerApellido = dto.PrimerApellido.Trim(),
                SegundoApellido = dto.SegundoApellido?.Trim(),
                FechaNacimiento = dto.FechaNacimiento,
                Activo = dto.Activo
            };

            Votante votanteCreado =
                await _votanteRepository.AgregarAsync(nuevoVotante);

            return (
                new ResultadoOperacionDto
                {
                    Exitoso = true,
                    Mensaje = "El votante fue agregado correctamente."
                },
                ConvertirADto(votanteCreado)
            );
        }

        public async Task<ResultadoOperacionDto> ActualizarAsync(
            int id,
            ActualizarVotanteDto dto)
        {
            Votante? votanteExistente =
                await _votanteRepository.ObtenerPorIdAsync(id);

            if (votanteExistente is null)
            {
                return new ResultadoOperacionDto
                {
                    Exitoso = false,
                    Mensaje = "No se encontró el votante."
                };
            }

            bool cedulaDuplicada =
                await _votanteRepository.ExisteCedulaAsync(dto.Cedula, id);

            if (cedulaDuplicada)
            {
                return new ResultadoOperacionDto
                {
                    Exitoso = false,
                    Mensaje = "Ya existe otro votante con esa cédula."
                };
            }

            if (dto.FechaNacimiento.Date > DateTime.Today)
            {
                return new ResultadoOperacionDto
                {
                    Exitoso = false,
                    Mensaje = "La fecha de nacimiento no puede ser futura."
                };
            }

            Votante votanteActualizado = new()
            {
                VotanteId = id,
                Cedula = dto.Cedula.Trim(),
                Nombre = dto.Nombre.Trim(),
                PrimerApellido = dto.PrimerApellido.Trim(),
                SegundoApellido = dto.SegundoApellido?.Trim(),
                FechaNacimiento = dto.FechaNacimiento,
                Activo = dto.Activo
            };

            bool actualizado =
                await _votanteRepository.ActualizarAsync(votanteActualizado);

            return new ResultadoOperacionDto
            {
                Exitoso = actualizado,
                Mensaje = actualizado
                    ? "El votante fue actualizado correctamente."
                    : "No se pudo actualizar el votante."
            };
        }

        public async Task<ResultadoOperacionDto> EliminarAsync(int id)
        {
            Votante? votante =
                await _votanteRepository.ObtenerPorIdAsync(id);

            if (votante is null)
            {
                return new ResultadoOperacionDto
                {
                    Exitoso = false,
                    Mensaje = "No se encontró el votante."
                };
            }

            bool eliminado =
                await _votanteRepository.EliminarAsync(id);

            return new ResultadoOperacionDto
            {
                Exitoso = eliminado,
                Mensaje = eliminado
                    ? "El votante fue eliminado correctamente."
                    : "No se pudo eliminar el votante."
            };
        }

        private static VotanteDto ConvertirADto(Votante votante)
        {
            return new VotanteDto
            {
                VotanteId = votante.VotanteId,
                Cedula = votante.Cedula,
                Nombre = votante.Nombre,
                PrimerApellido = votante.PrimerApellido,
                SegundoApellido = votante.SegundoApellido,
                FechaNacimiento = votante.FechaNacimiento,
                Activo = votante.Activo
            };
        }
    }
}