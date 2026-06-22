using System;
using System.Linq;
using System.Threading.Tasks;
using Servixa.Abstractions.Interfaces;
using Servixa.Shared.DTOs.Payment;
using Servixa.Domain.Models.PaymentEntity;
using Servixa.Domain.Contracts.UnitOfWorkPattern;
using Servixa.Shared.Commen.Responses;
using Servixa.Shared.Enums;

namespace Servixa.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<PaymentResponseDto>> CreatePaymentAsync(CreatePaymentDto dto)
        {
            var paymentRepo = _unitOfWork.GetReposatory<Payment, int>();

            var payment = new Payment
            {
                BookingId = dto.BookingId,
                Amount = dto.Amount,
                Method = dto.Method,
                Status = PaymentStatus.Pending
            };

            await paymentRepo.AddAsync(payment);
            await _unitOfWork.SaveChangesAsync();

            var response = new PaymentResponseDto
            {
                Id = payment.Id,
                BookingId = payment.BookingId,
                Amount = payment.Amount,
                Method = payment.Method,
                Status = payment.Status,
                CreatedAt = payment.CreatedAt
            };

            return new ApiResponse<PaymentResponseDto>(response, "Payment created");
        }

        public async Task<ApiResponse<PaymentResponseDto>> GetPaymentByBookingAsync(int bookingId)
        {
            var paymentRepo = _unitOfWork.GetReposatory<Payment, int>();
            var allPayments = await paymentRepo.GetAllAsync();
            var payment = allPayments.FirstOrDefault(p => p.BookingId == bookingId);

            if (payment == null) return new ApiResponse<PaymentResponseDto>(null!, "Not found", 404);

            var response = new PaymentResponseDto
            {
                Id = payment.Id,
                BookingId = payment.BookingId,
                Amount = payment.Amount,
                Method = payment.Method,
                Status = payment.Status,
                CreatedAt = payment.CreatedAt
            };

            return new ApiResponse<PaymentResponseDto>(response, "Ok");
        }

        public async Task<ApiResponse<bool>> UpdatePaymentStatusAsync(int paymentId, PaymentStatus status)
        {
            var paymentRepo = _unitOfWork.GetReposatory<Payment, int>();
            var payment = await paymentRepo.GetByIdAsync(paymentId);
            
            if (payment == null) return new ApiResponse<bool>(false, "Not found", 404);

            payment.Status = status;
            paymentRepo.UpdateAsync(payment);
            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse<bool>(true, "Updated successfully");
        }
    }
}
