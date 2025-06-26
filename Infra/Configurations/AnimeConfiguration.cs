using Infra.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Configurations;

public class AnimeConfiguration : IEntityTypeConfiguration<Anime>
{
    public void Configure(EntityTypeBuilder<Anime> builder)
    {
        builder.Property(a => a.Titulo)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.Descricao)
            .HasMaxLength(1000);

        builder.HasOne(a => a.Estudio)
            .WithMany(e => e.Animes)
            .HasForeignKey(a => a.EstudioId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Genero)
            .WithMany(g => g.Animes)
            .HasForeignKey(a => a.GeneroId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Ignore(a => a.MediaNota);  
    }
}
