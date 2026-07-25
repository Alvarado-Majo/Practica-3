using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VotacionNacional.DAL.Entities
{
    [Table("Votantes")]
    public class Votante
    {
        [Key]
        public int VotanteId { get; set; }

        [Required]
        [StringLength(20)]
        public string Cedula { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string PrimerApellido { get; set; } = string.Empty;

        [StringLength(100)]
        public string? SegundoApellido { get; set; }

        [Required]
        public DateTime FechaNacimiento { get; set; }

        public bool Activo { get; set; } = true;

        public Voto? Voto { get; set; }
    }
}