using Microsoft.AspNetCore.Http;

namespace Servixa.Shared.DTOs.Client
{
    public class UpdateClientProfileDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? DefaultAddress { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public IFormFile? ProfilePicture { get; set; }
    }
}
