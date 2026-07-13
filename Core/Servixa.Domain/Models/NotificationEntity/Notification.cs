using Servixa.Domain.Models.Users;
using Servixa.Domain.Models.BookingEntity;

namespace Servixa.Domain.Models.NotificationEntity
{
    public class Notification : BaseEntity<int>
    {
        public int ReceiverId { get; set; }
        public virtual ApplicationUser Receiver { get; set; } = null!;

        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public bool IsRead { get; set; } = false;

        public int? BookingId { get; set; }
        public virtual Booking? Booking { get; set; }
    }
}
