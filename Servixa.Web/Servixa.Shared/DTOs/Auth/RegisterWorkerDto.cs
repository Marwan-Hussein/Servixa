using Microsoft.AspNetCore.Http;
using System;

namespace Servixa.Shared.DTOs.Auth
{
    public class RegisterWorkerDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        
        public IFormFile? NationalIdFront { get; set; }
        public IFormFile? NationalIdBack { get; set; }
        public IFormFile? ProfilePicture { get; set; }
    }
}
