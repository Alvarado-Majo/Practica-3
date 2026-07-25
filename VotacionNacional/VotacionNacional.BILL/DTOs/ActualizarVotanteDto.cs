using System.ComponentModel.DataAnnotations;

namespace VotacionNacional.BLL.DTOs
{
    public class ActualizarVotanteDto
    {
        [Required(ErrorMessage = "La cédula es obligatoria.")]
        [StringLength(20)]
        public string Cedula { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El primer apellido es obligatorio.")]
        [StringLength(100)]
        public string PrimerApellido { get; set; } = string.Empty;

        [StringLength(100)]
        public string? SegundoApellido { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        public DateTime FechaNacimiento { get; set; }

        public bool Activo { get; set; }
    }
}