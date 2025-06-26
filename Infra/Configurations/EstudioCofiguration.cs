using Infra.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Configurations;

public class EstudioCofiguration : IEntityTypeConfiguration<Estudio>
{
    public void Configure(EntityTypeBuilder<Estudio> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Nome)
            .HasMaxLength(20)
            .IsRequired();
        builder.Property(e => e.Descricao)
            .HasMaxLength(100)
            .IsRequired();
        builder.HasMany(e => e.Animes)
            .WithOne(a => a.Estudio)
            .HasForeignKey(a => a.EstudioId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}