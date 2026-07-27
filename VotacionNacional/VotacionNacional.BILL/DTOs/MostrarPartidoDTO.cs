using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VotacionNacional.BLL.DTOs
{
    public class MostrarPartidoDTO
    {
        public int PartidoId { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Siglas { get; set; } = string.Empty;

        public string? ImagenUrl { get; set; }

        public int Votos { get; set; }

        public bool Activo { get; set; }
    }
}