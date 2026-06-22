using System.Threading.Tasks;
using Servixa.Shared.DTOs.Payment;
using Servixa.Shared.Commen.Responses;
using Servixa.Shared.Enums;

namespace Servixa.Abstractions.Interfaces
{
    public interface IPaymentService
    {
        Task<ApiResponse<PaymentResponseDto>> CreatePaymentAsync(CreatePaymentDto dto);
        Task<ApiResponse<PaymentResponseDto>> GetPaymentByBookingAsync(int bookingId);
        Task<ApiResponse<bool>> UpdatePaymentStatusAsync(int paymentId, PaymentStatus status);
    }
}
