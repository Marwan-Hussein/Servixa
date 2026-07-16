using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Servixa.Abstractions.Interfaces;
using Servixa.Shared.DTOs.Booking;
using Servixa.Shared.DTOs.Client;
using Servixa.Shared.DTOs.Review;

namespace Servixa.Presentation.Controllers
{
    /// <summary>
    /// All endpoints here require an authenticated Client role.
    /// The client's identity is always extracted from the JWT token — never from the request body.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Client")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly IWorkerService _workerService;

        public ClientController(IClientService clientService, IWorkerService workerService)
        {
            _clientService = clientService;
            _workerService = workerService;
        }

        // ── Helper ─────────────────────────────────────────────────────────────

        private int GetCurrentClientId()
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(claim!);
        }

        // ── Profile ────────────────────────────────────────────────────────────

        /// <summary>Get the authenticated client's own profile.</summary>
        [HttpGet("profile")]
        public async Task<IActionResult> GetMyProfile()
        {
            var clientId = GetCurrentClientId();
            var result = await _clientService.GetClientProfileAsync(clientId);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>Update the authenticated client's profile (supports profile picture upload).</summary>
        [HttpPut("profile")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateMyProfile([FromForm] UpdateClientProfileDto dto)
        {
            var clientId = GetCurrentClientId();
            var result = await _clientService.UpdateClientProfileAsync(clientId, dto);
            return StatusCode(result.StatusCode, result);
        }

        // ── Discover workers ───────────────────────────────────────────────────

        /// <summary>Browse workers near the client's location.</summary>
        [HttpGet("workers/nearby")]
        public async Task<IActionResult> GetNearbyWorkers(
            [FromQuery] decimal lat,
            [FromQuery] decimal lng,
            [FromQuery] double radiusInKm = 10)
        {
            var result = await _workerService.GetNearbyWorkersAsync(lat, lng, radiusInKm);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>Browse workers by specialty / service category.</summary>
        [HttpGet("workers/specialty/{specialtyId}")]
        public async Task<IActionResult> GetWorkersBySpecialty(int specialtyId)
        {
            var result = await _workerService.GetWorkersBySpecialtyAsync(specialtyId);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>View a specific worker's full profile before booking.</summary>
        [HttpGet("workers/{workerId}")]
        public async Task<IActionResult> GetWorkerProfile(int workerId)
        {
            var result = await _workerService.GetWorkerProfileAsync(workerId);
            return StatusCode(result.StatusCode, result);
        }

        // ── Bookings ────────────────────────────────────────────────────────────

        /// <summary>
        /// Book a task from a specific worker.
        /// The body must specify WorkerId, TaskId, ScheduledDate, and location.
        /// The ClientId is automatically set from the JWT token.
        /// </summary>
        [HttpPost("bookings")]
        public async Task<IActionResult> BookTask([FromBody] CreateBookingDto dto)
        {
            var clientId = GetCurrentClientId();
            var result = await _clientService.BookTaskAsync(clientId, dto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>List all bookings belonging to the authenticated client.</summary>
        [HttpGet("bookings")]
        public async Task<IActionResult> GetMyBookings()
        {
            var clientId = GetCurrentClientId();
            var result = await _clientService.GetMyBookingsAsync(clientId);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>Get details of a specific booking (must belong to this client).</summary>
        [HttpGet("bookings/{bookingId}")]
        public async Task<IActionResult> GetBookingDetails(int bookingId)
        {
            var clientId = GetCurrentClientId();
            var result = await _clientService.GetBookingDetailsAsync(clientId, bookingId);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>Cancel a pending booking (must belong to this client).</summary>
        [HttpDelete("bookings/{bookingId}/cancel")]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            var clientId = GetCurrentClientId();
            var result = await _clientService.CancelMyBookingAsync(clientId, bookingId);
            return StatusCode(result.StatusCode, result);
        }

        // ── Reviews ─────────────────────────────────────────────────────────────

        /// <summary>
        /// Leave a review for a completed booking.
        /// Only the client who made the booking can review it, and only once.
        /// </summary>
        [HttpPost("reviews")]
        public async Task<IActionResult> LeaveReview([FromBody] CreateReviewDto dto)
        {
            var clientId = GetCurrentClientId();
            var result = await _clientService.LeaveReviewAsync(clientId, dto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>List all reviews the authenticated client has written.</summary>
        [HttpGet("reviews")]
        public async Task<IActionResult> GetMyReviews()
        {
            var clientId = GetCurrentClientId();
            var result = await _clientService.GetMyReviewsAsync(clientId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
