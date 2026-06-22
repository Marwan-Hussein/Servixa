using System;

namespace Servixa.Shared.DTOs.Chat
{
    public class ChatMessageDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Message { get; set; }
        public string? Response { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
