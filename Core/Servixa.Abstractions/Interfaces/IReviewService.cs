using System.Collections.Generic;
using System.Threading.Tasks;
using Servixa.Shared.DTOs.Review;
using Servixa.Shared.Commen.Responses;

namespace Servixa.Abstractions.Interfaces
{
    public interface IReviewService
    {
        Task<ApiResponse<ReviewResponseDto>> CreateReviewAsync(CreateReviewDto dto);
        Task<ApiResponse<IEnumerable<ReviewResponseDto>>> GetReviewsByWorkerAsync(int workerId);
        Task<ApiResponse<IEnumerable<ReviewResponseDto>>> GetReviewsByBookingAsync(int bookingId);
    }
}
