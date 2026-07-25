using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VotacionNacional.DAL.Entities
{
    [Table("Votos")]
    public class Voto
    {
        [Key]
        public int VotoId { get; set; }

        [Required]
        public int VotanteId { get; set; }

        [Required]
        public int PartidoId { get; set; }

        public DateTime FechaVoto { get; set; } = DateTime.Now;

        [ForeignKey(nameof(VotanteId))]
        public Votante Votante { get; set; } = null!;

        [ForeignKey(nameof(PartidoId))]
        public Partido Partido { get; set; } = null!;
    }
}