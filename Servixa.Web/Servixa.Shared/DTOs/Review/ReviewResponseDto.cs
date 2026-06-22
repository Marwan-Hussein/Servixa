using System;

namespace Servixa.Shared.DTOs.Review
{
    public class ReviewResponseDto
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int ReviewerId { get; set; }
        public int RevieweeId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
