using Servixa.Domain.Models.NotificationEntity;
using Servixa.Domain.Models.ReviewEntity;
using Servixa.Domain.Models.BookingEntity;
using System.Collections.Generic;

namespace Servixa.Domain.Models.Users
{
    public class Client : ApplicationUser
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? DefaultAddress { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
        public virtual ICollection<Review> GivenReviews { get; set; } = new HashSet<Review>();
        public virtual ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();
    }
}
