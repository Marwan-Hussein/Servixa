namespace Servixa.Shared.DTOs.Worker
{
    public class WorkerDetailResponseDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public int SpecialtyId { get; set; }
        public double AverageRating { get; set; }
        public bool IsAvailable { get; set; }
        public string? Status { get; set; }
        public string? NationalIdFront { get; set; }
        public string? NationalIdBack { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsVerified { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? NationalIdFrontUrl { get; set; }
        public string? NationalIdBackUrl { get; set; }
    }
}
