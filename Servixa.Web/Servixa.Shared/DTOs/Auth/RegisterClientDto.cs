using System;

namespace Servixa.Shared.DTOs.Auth
{
    public class RegisterClientDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? DefaultAddress { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
