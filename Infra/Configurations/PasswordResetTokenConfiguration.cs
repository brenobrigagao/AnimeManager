using Infra.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurations
{
    public class PasswordResetTokenConfiguration : IEntityTypeConfiguration<PasswordResetToken>
    {
        public void Configure(EntityTypeBuilder<PasswordResetToken> builder)
        {
            builder.ToTable("PasswordResetTokens");

            builder.HasOne(p => p.Usuario)
                .WithMany()
                .HasForeignKey(p => p.usuarioId);

            builder.Property(p => p.Token)
                .IsRequired();

            builder.Property(p => p.Expires)
                .IsRequired();

            builder.Property(p => p.Usado)
                .HasDefaultValue(false);
        }
    }
}