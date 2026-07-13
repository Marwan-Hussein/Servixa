using System;
using Servixa.Shared.Enums;

namespace Servixa.Shared.DTOs.Payment
{
    public class PaymentResponseDto
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; }
        public PaymentMethod Method { get; set; }
        public string? TransactionId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
