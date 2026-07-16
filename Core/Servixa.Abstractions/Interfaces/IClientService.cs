using System.Collections.Generic;
using System.Threading.Tasks;
using Servixa.Shared.DTOs.Client;
using Servixa.Shared.DTOs.Booking;
using Servixa.Shared.DTOs.Review;
using Servixa.Shared.Commen.Responses;

namespace Servixa.Abstractions.Interfaces
{
    public interface IClientService
    {
        // ── Profile ──────────────────────────────────────────────────────────
        Task<ApiResponse<ClientResponseDto>> GetClientProfileAsync(int clientId);
        Task<ApiResponse<ClientResponseDto>> UpdateClientProfileAsync(int clientId, UpdateClientProfileDto dto);

        // ── Bookings ──────────────────────────────────────────────────────────
        /// <summary>Book a task from a specific worker.</summary>
        Task<ApiResponse<BookingResponseDto>> BookTaskAsync(int clientId, CreateBookingDto dto);
        Task<ApiResponse<IEnumerable<BookingResponseDto>>> GetMyBookingsAsync(int clientId);
        Task<ApiResponse<BookingResponseDto>> GetBookingDetailsAsync(int clientId, int bookingId);
        Task<ApiResponse<bool>> CancelMyBookingAsync(int clientId, int bookingId);

        // ── Reviews ───────────────────────────────────────────────────────────
        /// <summary>Leave a review for a completed booking.</summary>
        Task<ApiResponse<ReviewResponseDto>> LeaveReviewAsync(int clientId, CreateReviewDto dto);
        Task<ApiResponse<IEnumerable<ReviewResponseDto>>> GetMyReviewsAsync(int clientId);
    }
}
