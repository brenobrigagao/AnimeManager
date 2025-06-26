using Infra.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.HasKey(u => u.Id);
        builder
            .Property(u => u.Nome)
            .HasMaxLength(30)
            .IsRequired();
        builder.Property(u => u.Email)
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(u => u.Senha)
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(u => u.IsAdmin)
            .IsRequired();
        builder.HasMany(u => u.Avaliacoes)
            .WithOne(a => a.Usuario)
            .HasForeignKey(a => a.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(u => u.Email).IsUnique();
    }
}