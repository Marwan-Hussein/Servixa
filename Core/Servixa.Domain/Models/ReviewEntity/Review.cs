using Servixa.Domain.Models.Users;
using Servixa.Domain.Models.BookingEntity;

namespace Servixa.Domain.Models.ReviewEntity
{
    public class Review : BaseEntity<int>
    {
        public int BookingId { get; set; }
        public virtual Booking Booking { get; set; } = null!;

        public int ReviewerId { get; set; }
        public virtual ApplicationUser Reviewer { get; set; } = null!;

        public int RevieweeId { get; set; }
        public virtual ApplicationUser Reviewee { get; set; } = null!;

        public int Rating { get; set; } 
        public string? Comment { get; set; }
    }
}
