namespace Servixa.Shared.DTOs.Review
{
    public class CreateReviewDto
    {
        public int BookingId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
