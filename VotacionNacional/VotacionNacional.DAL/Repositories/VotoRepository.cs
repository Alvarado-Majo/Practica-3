using Microsoft.EntityFrameworkCore;
using VotacionNacional.DAL.Context;
using VotacionNacional.DAL.Entities;
using VotacionNacional.DAL.Interfaces;

namespace VotacionNacional.DAL.Repositories
{
    public class VotoRepository : IVotoRepository
    {
        private readonly VotacionDbContext _context;

        public VotoRepository(VotacionDbContext context)
        {
            _context = context;
        }

        public async Task<Voto> RegistrarVotoAsync(Voto voto)
        {
            _context.Votos.Add(voto);

            await _context.SaveChangesAsync();

            return voto;
        }

        public async Task<bool> VotanteYaVotoAsync(int votanteId)
        {
            return await _context.Votos
                .AnyAsync(v => v.VotanteId == votanteId);
        }

        public async Task<List<Voto>> ObtenerTodosAsync()
        {
            return await _context.Votos
                .Include(v => v.Votante)
                .Include(v => v.Partido)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}