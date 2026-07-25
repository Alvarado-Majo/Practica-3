using Microsoft.EntityFrameworkCore;
using VotacionNacional.DAL.Context;
using VotacionNacional.DAL.Entities;
using VotacionNacional.DAL.Interfaces;

namespace VotacionNacional.DAL.Repositories
{
    public class VotanteRepository : IVotanteRepository
    {
        private readonly VotacionDbContext _context;

        public VotanteRepository(VotacionDbContext context)
        {
            _context = context;
        }

        public async Task<List<Votante>> ObtenerTodosAsync()
        {
            return await _context.Votantes
                .AsNoTracking()
                .OrderBy(v => v.PrimerApellido)
                .ThenBy(v => v.Nombre)
                .ToListAsync();
        }

        public async Task<Votante?> ObtenerPorIdAsync(int id)
        {
            return await _context.Votantes
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.VotanteId == id);
        }

        public async Task<Votante?> ObtenerPorCedulaAsync(string cedula)
        {
            return await _context.Votantes
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.Cedula == cedula);
        }

        public async Task<bool> ExisteCedulaAsync(
            string cedula,
            int? excluirId = null)
        {
            return await _context.Votantes.AnyAsync(v =>
                v.Cedula == cedula &&
                (!excluirId.HasValue || v.VotanteId != excluirId.Value));
        }

        public async Task<Votante> AgregarAsync(Votante votante)
        {
            _context.Votantes.Add(votante);
            await _context.SaveChangesAsync();

            return votante;
        }

        public async Task<bool> ActualizarAsync(Votante votante)
        {
            Votante? votanteExistente = await _context.Votantes
                .FirstOrDefaultAsync(v => v.VotanteId == votante.VotanteId);

            if (votanteExistente is null)
            {
                return false;
            }

            votanteExistente.Cedula = votante.Cedula;
            votanteExistente.Nombre = votante.Nombre;
            votanteExistente.PrimerApellido = votante.PrimerApellido;
            votanteExistente.SegundoApellido = votante.SegundoApellido;
            votanteExistente.FechaNacimiento = votante.FechaNacimiento;
            votanteExistente.Activo = votante.Activo;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            Votante? votante = await _context.Votantes
                .FirstOrDefaultAsync(v => v.VotanteId == id);

            if (votante is null)
            {
                return false;
            }

            _context.Votantes.Remove(votante);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
