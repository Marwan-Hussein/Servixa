using System;

namespace Servixa.Shared.DTOs.Notification
{
    public class NotificationResponseDto
    {
        public int Id { get; set; }
        public int ReceiverId { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
