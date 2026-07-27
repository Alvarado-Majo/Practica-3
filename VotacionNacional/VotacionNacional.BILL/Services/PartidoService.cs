using System;
using System.Collections.Generic;
using System.Text;
using VotacionNacional.BLL.DTOs;
using VotacionNacional.BLL.Interfaces;
using VotacionNacional.DAL.Entities;
using VotacionNacional.DAL.Interfaces;

namespace VotacionNacional.BLL.Services
{
    public class PartidoService : IPartidoService
    {
        private readonly IPartidoRepository _partidoRepository;
        public PartidoService(IPartidoRepository partidoRepository)
        {
            _partidoRepository = partidoRepository;
        }
        public async Task<ResultadoOperacionDto> CreatePartidoAsync(CrearPartidoDTO partidoDto)
        {
            var respuesta = new ResultadoOperacionDto();
            var nombreExistente = await _partidoRepository.ObtenerPartidosPorNombreAsync(partidoDto.Nombre);
            if (nombreExistente != null)
            {
                respuesta.Mensaje = "El nombre del partido ya existe";
                respuesta.Exitoso = false;
                return respuesta;
            }
            var partidoExistente = await _partidoRepository.ObtenerPartidosPorSiglasAsync(partidoDto.Siglas);
            if (partidoExistente != null)
            {
                respuesta.Mensaje = "El partido ya existe";
                respuesta.Exitoso = false;
                return respuesta;
            }
            var partido = new Partido
            {
                Siglas = partidoDto.Siglas,
                Nombre = partidoDto.Nombre,
                ImagenUrl = partidoDto.ImagenUrl,
                Activo = true
            };
            var resultado = await _partidoRepository.AgregarPartidoAsync(partido);
            if (!resultado)
            {
                respuesta.Mensaje = "No se pudo crear el partido";
                respuesta.Exitoso = false;
                return respuesta;
            }
            respuesta.Mensaje = "Partido creado correctamente";
            respuesta.Exitoso = true;
            return respuesta;
        }

        public async Task<bool> DeletePartidoByIdAsync(int id)
        {
            return await _partidoRepository.EliminarPartidoAsync(id);
        }

        public async Task<List<MostrarPartidoDTO>> GetAllPartidosAsync()
        {
            var entidades =
                await _partidoRepository.ObtenerPartidosAsync();

            return entidades.Select(x => new MostrarPartidoDTO
            {
                PartidoId = x.PartidoId,
                Nombre = x.Nombre,
                Siglas = x.Siglas,
                ImagenUrl = x.ImagenUrl,
                Activo = x.Activo,
                Votos = x.Votos.Count
            }).ToList();
        }

        public async Task<MostrarPartidoDTO?> GetPartidoByIdAsync(int id)
        {
            var partido =
                await _partidoRepository.ObtenerPartidosPorIdAsync(id);

            if (partido == null)
                return null;

            return new MostrarPartidoDTO
            {
                PartidoId = partido.PartidoId,
                Nombre = partido.Nombre,
                Siglas = partido.Siglas,
                ImagenUrl = partido.ImagenUrl,
                Activo = partido.Activo,
                Votos = partido.Votos.Count
            };
        }

        public async Task<MostrarPartidoDTO?> GetPartidoByNombreAsync(string nombre)
        {
            var partido =
                await _partidoRepository.ObtenerPartidosPorNombreAsync(nombre);

            if (partido == null)
                return null;

            return new MostrarPartidoDTO
            {
                PartidoId = partido.PartidoId,
                Nombre = partido.Nombre,
                Siglas = partido.Siglas,
                ImagenUrl = partido.ImagenUrl,
                Activo = partido.Activo,
                Votos = partido.Votos.Count
            };
        }

        public async Task<MostrarPartidoDTO?> GetPartidoBySiglasAsync(string siglas)
        {
            var partido =
                await _partidoRepository.ObtenerPartidosPorSiglasAsync(siglas);

            if (partido == null)
                return null;

            return new MostrarPartidoDTO
            {
                PartidoId = partido.PartidoId,
                Nombre = partido.Nombre,
                Siglas = partido.Siglas,
                ImagenUrl = partido.ImagenUrl,
                Activo = partido.Activo,
                Votos = partido.Votos.Count
            };
        }

        public async Task<ResultadoOperacionDto> UpdatePartidoAsync(int id, ActualizarPartidoDTO partidoDto)
        {
            var respuesta = new ResultadoOperacionDto();
            var partidoExistente = await _partidoRepository.ObtenerPartidosPorIdAsync(id);
            if (partidoExistente == null)
            {
                respuesta.Mensaje = "El partido no existe";
                respuesta.Exitoso = false;
                return respuesta;
            }
            partidoExistente.Nombre = partidoDto.Nombre;
            var partidoConMismoNombre = await _partidoRepository.ObtenerPartidosPorNombreAsync(partidoDto.Nombre);
            if (partidoConMismoNombre != null && partidoConMismoNombre.PartidoId != id)
            {
                respuesta.Mensaje = "El nombre del partido ya existe";
                respuesta.Exitoso = false;
                return respuesta;
            }
            partidoExistente.Siglas = partidoDto.Siglas;
            var partidoConMismasSiglas = await _partidoRepository.ObtenerPartidosPorSiglasAsync(partidoDto.Siglas);
            if (partidoConMismasSiglas != null && partidoConMismasSiglas.PartidoId != id)
            {
                respuesta.Mensaje = "Las siglas del partido ya existen";
                respuesta.Exitoso = false;
                return respuesta;
            }
            partidoExistente.ImagenUrl = partidoDto.ImagenUrl;
            partidoExistente.Activo = partidoDto.Activo;
            var resultado = await _partidoRepository.ActualizarPartidoAsync(partidoExistente);
            if (!resultado)
            {
                respuesta.Mensaje = "No se pudo actualizar el partido";
                respuesta.Exitoso = false;
                return respuesta;
            }
            respuesta.Mensaje = "Partido actualizado correctamente";
            respuesta.Exitoso = true;
            return respuesta;
        }
    }
}
