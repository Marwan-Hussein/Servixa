using Servixa.Domain.Models.Users;

namespace Servixa.Domain.Models.OtpEntity
{
    public class OtpCode : BaseEntity<int>
    {
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string CodeHash { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; }
        public DateTime? UsedAt { get; set; }
        public virtual ApplicationUser User { get; set; } = null!;
    }
}
