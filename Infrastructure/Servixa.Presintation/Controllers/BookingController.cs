using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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

        /// <summary>Worker or admin: view a single booking by its ID.</summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var result = await _bookingService.GetBookingByIdAsync(id);
            return Ok(result);
        }

        /// <summary>Get all bookings for a specific client (admin or own client).</summary>
        [HttpGet("client/{clientId}")]
        public async Task<IActionResult> GetClientBookings(int clientId)
        {
            var result = await _bookingService.GetClientBookingsAsync(clientId);
            return Ok(result);
        }

        /// <summary>Get all bookings assigned to a worker.</summary>
        [HttpGet("worker/{workerId}")]
        public async Task<IActionResult> GetWorkerBookings(int workerId)
        {
            var result = await _bookingService.GetWorkerBookingsAsync(workerId);
            return Ok(result);
        }

        /// <summary>Worker updates booking status (Accepted, InProgress, Completed).</summary>
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateBookingStatus(int id, [FromBody] UpdateBookingStatusDto dto)
        {
            var result = await _bookingService.UpdateBookingStatusAsync(id, dto);
            return Ok(result);
        }

        /// <summary>Cancel a booking (used by admin or direct route).</summary>
        [HttpDelete("{id}/cancel")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var result = await _bookingService.CancelBookingAsync(id);
            return Ok(result);
        }
    }
}
