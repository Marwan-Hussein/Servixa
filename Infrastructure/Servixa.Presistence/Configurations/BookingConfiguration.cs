using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servixa.Domain.Models.BookingEntity;

namespace Servixa.Presistence.Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.ToTable("Booking");
            builder.HasKey(b => b.Id);
            
            builder.Property(b => b.FinalCost).HasColumnType("decimal(18,2)");
            
            builder.HasOne(b => b.Client)
                   .WithMany(c => c.Bookings)
                   .HasForeignKey(b => b.ClientId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(b => b.Worker)
                   .WithMany(w => w.AssignedBookings)
                   .HasForeignKey(b => b.WorkerId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(b => b.Task)
                   .WithMany()
                   .HasForeignKey(b => b.TaskId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Payment)
                   .WithOne(p => p.Booking)
                   .HasForeignKey<Servixa.Domain.Models.PaymentEntity.Payment>(p => p.BookingId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
