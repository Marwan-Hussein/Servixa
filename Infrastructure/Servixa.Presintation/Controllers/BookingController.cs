using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Servixa.Abstractions.Interfaces;
using Servixa.Shared.DTOs.Booking;

namespace Servixa.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDto dto)
        {
            var result = await _bookingService.CreateBookingAsync(dto);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var result = await _bookingService.GetBookingByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("client/{clientId}")]
        public async Task<IActionResult> GetClientBookings(int clientId)
        {
            var result = await _bookingService.GetClientBookingsAsync(clientId);
            return Ok(result);
        }

        [HttpGet("worker/{workerId}")]
        public async Task<IActionResult> GetWorkerBookings(int workerId)
        {
            var result = await _bookingService.GetWorkerBookingsAsync(workerId);
            return Ok(result);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateBookingStatus(int id, [FromBody] UpdateBookingStatusDto dto)
        {
            var result = await _bookingService.UpdateBookingStatusAsync(id, dto);
            return Ok(result);
        }

        [HttpDelete("{id}/cancel")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var result = await _bookingService.CancelBookingAsync(id);
            return Ok(result);
        }
    }
}
