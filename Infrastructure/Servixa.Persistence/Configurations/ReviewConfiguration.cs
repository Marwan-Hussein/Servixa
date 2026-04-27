using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servixa.Domain.Models.ReviewEntity;

namespace Servixa.Persistence.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("Review");
            builder.HasKey(r => r.Id);

            builder.HasOne(r => r.Booking)
                   .WithMany(b => b.Reviews)
                   .HasForeignKey(r => r.BookingId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.Reviewer)
                   .WithMany()
                   .HasForeignKey(r => r.ReviewerId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(r => r.Reviewee)
                   .WithMany()
                   .HasForeignKey(r => r.RevieweeId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
