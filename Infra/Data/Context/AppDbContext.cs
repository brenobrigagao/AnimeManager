using Infra.Configurations;
using Infra.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using Infra.Entities;

namespace Infra.Data.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    public DbSet<Anime> Animes { get; set; }
    public DbSet<Avaliacao> Avaliacoes { get; set; }
    public DbSet<Estudio> Estudios { get; set; }
    public DbSet<Genero> Generos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new PasswordResetTokenConfiguration());
        modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
        modelBuilder.ApplyConfiguration(new AvaliacaoConfiguration());
        modelBuilder.ApplyConfiguration(new EstudioCofiguration());
        modelBuilder.ApplyConfiguration(new GeneroConfiguration());
        modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
        modelBuilder.ApplyConfiguration(new AnimeConfiguration());
        
    }
}