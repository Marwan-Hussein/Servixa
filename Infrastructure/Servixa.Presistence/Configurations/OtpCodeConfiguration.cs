using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servixa.Domain.Models.OtpEntity;

namespace Servixa.Presistence.Configurations
{
    public class OtpCodeConfiguration : IEntityTypeConfiguration<OtpCode>
    {
        public void Configure(EntityTypeBuilder<OtpCode> builder)
        {
            builder.ToTable("OtpCodes");
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Email)
                   .IsRequired()
                   .HasMaxLength(256);

            builder.Property(o => o.CodeHash)
                   .IsRequired()
                   .HasMaxLength(128);

            builder.Property(o => o.Purpose)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasIndex(o => new { o.UserId, o.Purpose, o.IsUsed, o.ExpiresAt });

            builder.HasOne(o => o.User)
                   .WithMany()
                   .HasForeignKey(o => o.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
