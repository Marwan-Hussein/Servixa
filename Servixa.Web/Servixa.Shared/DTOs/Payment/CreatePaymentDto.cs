using Servixa.Shared.Enums;

namespace Servixa.Shared.DTOs.Payment
{
    public class CreatePaymentDto
    {
        public int BookingId { get; set; }
        public PaymentMethod Method { get; set; }
        public decimal Amount { get; set; } = 0;
    }
}
