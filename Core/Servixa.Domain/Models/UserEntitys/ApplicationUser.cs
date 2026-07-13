using Microsoft.AspNetCore.Identity;
using Servixa.Domain.Contracts;

namespace Servixa.Domain.Models.Users
{
    public class ApplicationUser : IdentityUser<int> , IEntity<int>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? ProfilePictureUrl { get; set; }
        public bool IsDeleted { get; set; }
    }
}
