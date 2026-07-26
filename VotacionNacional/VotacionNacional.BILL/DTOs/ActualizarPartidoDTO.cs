using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VotacionNacional.BLL.DTOs
{
    public class ActualizarPartidoDTO
    {
 

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Las siglas son obligatorias")]
        [StringLength(20)]
        public string Siglas { get; set; } = string.Empty;

        [Url(ErrorMessage = "La URL de la imagen no es válida")]
        [Required(ErrorMessage = "La URL de la imagen es obligatoria")]
        [StringLength(250)]
        public string? ImagenUrl { get; set; }

        public bool Activo { get; set; } = true;

    }
}
