namespace VotacionNacional.BLL.DTOs
{
    public class VotanteDto
    {
        public int VotanteId { get; set; }

        public string Cedula { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public string PrimerApellido { get; set; } = string.Empty;

        public string? SegundoApellido { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public bool Activo { get; set; }
    }
}
