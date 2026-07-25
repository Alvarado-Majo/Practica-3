using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VotacionNacional.DAL.Entities
{
    [Table("Partidos")]
    public class Partido
    {
        [Key]
        public int PartidoId { get; set; }

        [Required]
        [StringLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Siglas { get; set; } = string.Empty;

        [StringLength(250)]
        public string? ImagenUrl { get; set; }

        public bool Activo { get; set; } = true;

        public ICollection<Voto> Votos { get; set; } = new List<Voto>();
    }
}