using Microsoft.EntityFrameworkCore;
using VotacionNacional.DAL.Entities;

namespace VotacionNacional.DAL.Context
{
    public class VotacionDbContext : DbContext
    {
        public VotacionDbContext(DbContextOptions<VotacionDbContext> options)
            : base(options)
        {
        }

        public DbSet<Votante> Votantes { get; set; }
        public DbSet<Partido> Partidos { get; set; }
        public DbSet<Voto> Votos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Votante>()
                .HasIndex(v => v.Cedula)
                .IsUnique();

            modelBuilder.Entity<Partido>()
                .HasIndex(p => p.Siglas)
                .IsUnique();

            modelBuilder.Entity<Voto>()
                .HasIndex(v => v.VotanteId)
                .IsUnique();

            modelBuilder.Entity<Voto>()
                .HasOne(v => v.Votante)
                .WithOne(v => v.Voto)
                .HasForeignKey<Voto>(v => v.VotanteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Voto>()
                .HasOne(v => v.Partido)
                .WithMany(p => p.Votos)
                .HasForeignKey(v => v.PartidoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
