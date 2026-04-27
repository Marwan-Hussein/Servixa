using Servixa.Domain.Enums;
using Servixa.Shared.Enums;
using Servixa.Domain.Models.BookingEntity;

namespace Servixa.Domain.Models.PaymentEntity
{
    public class Payment : BaseEntity<int>
    {
        public int BookingId { get; set; }
        public virtual Booking Booking { get; set; } = null!;

        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; } 
        public PaymentStatus Status { get; set; } 
    }
}
