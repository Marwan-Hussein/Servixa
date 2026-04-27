using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servixa.Domain.Models.NotificationEntity;

namespace Servixa.Persistence.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notification");
            builder.HasKey(n => n.Id);

            builder.HasOne(n => n.Receiver)
                   .WithMany()
                   .HasForeignKey(n => n.ReceiverId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(n => n.Booking)
                   .WithMany(b => b.Notifications)
                   .HasForeignKey(n => n.BookingId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
