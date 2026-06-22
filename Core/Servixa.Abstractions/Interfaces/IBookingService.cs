using System.Collections.Generic;
using System.Threading.Tasks;
using Servixa.Shared.DTOs.Booking;
using Servixa.Shared.Commen.Responses;

namespace Servixa.Abstractions.Interfaces
{
    public interface IBookingService
    {
        Task<ApiResponse<BookingResponseDto>> CreateBookingAsync(CreateBookingDto dto);
        Task<ApiResponse<BookingResponseDto>> GetBookingByIdAsync(int id);
        Task<ApiResponse<IEnumerable<BookingResponseDto>>> GetClientBookingsAsync(int clientId);
        Task<ApiResponse<IEnumerable<BookingResponseDto>>> GetWorkerBookingsAsync(int workerId);
        Task<ApiResponse<BookingResponseDto>> UpdateBookingStatusAsync(int id, UpdateBookingStatusDto dto);
        Task<ApiResponse<bool>> CancelBookingAsync(int id);
    }
}
