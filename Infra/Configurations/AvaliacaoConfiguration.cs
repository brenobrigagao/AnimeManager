using Infra.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Configurations;

public class AvaliacaoConfiguration : IEntityTypeConfiguration<Avaliacao>
{
    public void Configure(EntityTypeBuilder<Avaliacao> builder)
    {
        builder.Property(a => a.Nota)
            .IsRequired();
        
        builder.Property(a => a.Comentario)
            .HasMaxLength(1000);

        builder.HasOne(a => a.Usuario)
            .WithMany(u => u.Avaliacoes)
            .HasForeignKey(a => a.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Anime)
            .WithMany(an => an.Avaliacoes)
            .HasForeignKey(a => a.AnimeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
