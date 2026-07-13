using System;

namespace Servixa.Shared.DTOs.Booking
{
    public class CreateBookingDto
    {
        public int WorkerId { get; set; }
        public int TaskId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public decimal LocationLat { get; set; }
        public decimal LocationLng { get; set; }
        public string? Address { get; set; }
    }
}
