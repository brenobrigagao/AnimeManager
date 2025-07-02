using Infra.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder
            .Property(u => u.Nome)
            .HasMaxLength(30)
            .IsRequired();
        builder.Property(u => u.Email)
            .HasMaxLength(100)
            .IsRequired();
        builder.HasIndex(u => u.Email) 
            .IsUnique();

        builder.Property(u => u.SenhaHash)
            .IsRequired();

        builder.Property(u => u.SenhaSalt)
            .IsRequired(false); 
        
    }
}