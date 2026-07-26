
using Microsoft.EntityFrameworkCore;
using VotacionNacional.DAL.Context;
using VotacionNacional.DAL.Entities;
using VotacionNacional.DAL.Interfaces;

namespace VotacionNacional.DAL.Repositories
{
    public class PartidoRepository : IPartidoRepository

    {
        private readonly VotacionDbContext _context;
        public PartidoRepository(VotacionDbContext context)
        {
            _context = context;
        }
        public async Task<List<Partido>> ObtenerPartidosAsync()
        {
            return await _context.Partidos.OrderBy(p => p.Nombre)
                .ToListAsync();

        }

        public async Task<Partido?> ObtenerPartidosPorIdAsync(int id)
        {
            return await _context.Partidos.FirstOrDefaultAsync(p => p.PartidoId == id);
        }

        public async Task<bool> ActualizarPartidoAsync(Partido partido)
        {
            if (partido == null) {
                return false;
            }
            _context.Partidos.Update(partido);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AgregarPartidoAsync(Partido partido)
        {
            if (partido == null) {
                return false;
            }
            await _context.Partidos.AddAsync(partido);
            return await _context.SaveChangesAsync() > 0;
            
        }

        public async Task<bool> EliminarPartidoAsync(int id)
        {
            var partido = await _context.Partidos.FirstOrDefaultAsync(p => p.PartidoId == id);
            if (partido == null) {
                return false;
            }
            _context.Partidos.Remove(partido);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> ContarVotosPorPartidoAsync(int partidoId)
        {
            return await _context.Votos.CountAsync(v => v.PartidoId == partidoId);
        }

        public async Task<Partido?> ObtenerPartidosPorSiglasAsync(string siglas)
        {
            var partido = await _context.Partidos.FirstOrDefaultAsync(p => p.Siglas == siglas);
            if (partido == null) {
                return null;
            }
            return partido;
        }

        public async Task<Partido?> ObtenerPartidosPorNombreAsync(string nombre)
        {
            var partido = await _context.Partidos.FirstOrDefaultAsync(p => p.Nombre == nombre);
            if (partido == null) {
                return null;
            }
            return partido;
        }
    }
}
