using Servixa.Domain.Models.NotificationEntity;
using Servixa.Domain.Models.PaymentEntity;
using Servixa.Domain.Models.ReviewEntity;
using Servixa.Domain.Models.TaskEntity;
using Servixa.Domain.Models.Users;
using Servixa.Shared.Enums;
using System;
using System.Collections.Generic;

namespace Servixa.Domain.Models.BookingEntity
{
    public class Booking : BaseEntity<int>
    {
        public string ClientId { get; set; } = string.Empty;
        public virtual Client Client { get; set; } = null!;

        public string WorkerId { get; set; } = string.Empty;
        public virtual Worker Worker { get; set; } = null!;

        public int TaskId { get; set; }
        public virtual TaskEntity.Task Task { get; set; } = null!;
        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        public DateTime ScheduledDate { get; set; } 
        public decimal FinalCost { get; set; }

        public double LocationLatitude { get; set; }
        public double LocationLongitude { get; set; }
        public string? LocationAddress { get; set; }

        public virtual ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
        public virtual ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();
        public virtual Payment? Payment { get; set; } 
    }
}
