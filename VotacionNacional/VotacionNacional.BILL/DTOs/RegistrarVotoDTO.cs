using System.ComponentModel.DataAnnotations;

namespace VotacionNacional.BLL.DTOs
{
    public class RegistrarVotoDTO
    {
        [Required(ErrorMessage = "La cédula es obligatoria.")]
        [StringLength(20)]
        public string Cedula { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar un partido.")]
        public int PartidoId { get; set; }
    }
}