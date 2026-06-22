using Servixa.Domain.Models.NotificationEntity;
using Servixa.Domain.Models.BookingEntity;
using Servixa.Domain.Models.ReviewEntity;
using Servixa.Shared.Enums;
using System.Collections.Generic;

namespace Servixa.Domain.Models.Users
{
    public class Worker : ApplicationUser
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsAvailable { get; set; }
        public double AverageRating { get; set; } = 0;

        public string NationalIdFrontUrl { get; set; } = string.Empty;
        public string NationalIdBackUrl { get; set; } = string.Empty;
        public bool IsVerified { get; set; }
        public WorkerStatus Status { get; set; }

        public virtual ICollection<WorkerTask> WorkerTasks { get; set; } = new HashSet<WorkerTask>();
        public virtual ICollection<Booking> AssignedBookings { get; set; } = new HashSet<Booking>();
        public virtual ICollection<Review> ReceivedReviews { get; set; } = new HashSet<Review>();
        public virtual ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();
    }
}
