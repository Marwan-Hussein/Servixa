using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Servixa.Abstractions.Interfaces;
using Servixa.Shared.DTOs.Payment;
using Servixa.Domain.Models.PaymentEntity;
using Servixa.Shared.Enums;

namespace Servixa.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentDto dto)
        {
            var result = await _paymentService.CreatePaymentAsync(dto);
            return Ok(result);
        }

        [HttpGet("booking/{bookingId}")]
        public async Task<IActionResult> GetPaymentByBooking(int bookingId)
        {
            var result = await _paymentService.GetPaymentByBookingAsync(bookingId);
            return Ok(result);
        }

        [HttpPut("{paymentId}/status")]
        public async Task<IActionResult> UpdatePaymentStatus(int paymentId, [FromBody] PaymentStatus status)
        {
            var result = await _paymentService.UpdatePaymentStatusAsync(paymentId, status);
            return Ok(result);
        }
    }
}
