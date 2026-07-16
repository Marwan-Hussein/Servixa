namespace Servixa.Shared.DTOs.Client
{
    public class ClientResponseDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? DefaultAddress { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? ProfilePictureUrl { get; set; }
    }
}
