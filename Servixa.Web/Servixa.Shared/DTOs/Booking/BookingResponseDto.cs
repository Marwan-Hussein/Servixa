using System;

namespace Servixa.Shared.DTOs.Booking
{
    public class BookingResponseDto
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int WorkerId { get; set; }
        public int TaskId { get; set; }
        public string? Status { get; set; }
        public DateTime ScheduledDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Address { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
