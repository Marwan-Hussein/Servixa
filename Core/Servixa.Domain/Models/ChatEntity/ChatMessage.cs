using Servixa.Domain.Models.Users;
using System;

namespace Servixa.Domain.Models.ChatEntity
{
    public class ChatMessage : BaseEntity<int>
    {
        public int UserId { get; set; }
        public virtual ApplicationUser User { get; set; } = null!;
        
        public string Message { get; set; } = string.Empty;
        public string Response { get; set; } = string.Empty;
    }
}
