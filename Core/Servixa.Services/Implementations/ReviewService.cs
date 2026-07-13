using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Servixa.Abstractions.Interfaces;
using Servixa.Shared.DTOs.Review;
using Servixa.Domain.Contracts.UnitOfWorkPattern;
using Servixa.Shared.Commen.Responses;
using Servixa.Domain.Models.ReviewEntity;
using Servixa.Domain.Models.BookingEntity;
using Servixa.Domain.Contracts;
using Servixa.Domain.Specifications;

namespace Servixa.Services.Implementations
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReviewService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<ReviewResponseDto>> CreateReviewAsync(CreateReviewDto dto)
        {
            var bookingRepo = _unitOfWork.GetReposatory<Booking, int>();
            var booking = await bookingRepo.GetByIdAsync(dto.BookingId);

            if (booking == null)
            {
                return new ApiResponse<ReviewResponseDto>(null!, "Booking not found", 404);
            }

            var reviewRepo = _unitOfWork.GetReposatory<Review, int>();
            var review = new Review
            {
                BookingId = dto.BookingId,
                ReviewerId = booking.ClientId,
                RevieweeId = booking.WorkerId,
                Rating = dto.Rating,
                Comment = dto.Comment
            };

            await reviewRepo.AddAsync(review);
            await _unitOfWork.SaveChangesAsync();

            var responseDto = MapToResponseDto(review);
            return new ApiResponse<ReviewResponseDto>(responseDto, "Created");
        }

        public async Task<ApiResponse<IEnumerable<ReviewResponseDto>>> GetReviewsByWorkerAsync(int workerId)
        {
            var repo = _unitOfWork.GetReposatory<Review, int>();
            var spec = new ReviewWithDetailsSpecification(workerId);
            var reviews = await repo.GetAllWithSpecAsync(spec);

            var responseDtos = reviews.Select(MapToResponseDto).ToList();
            return new ApiResponse<IEnumerable<ReviewResponseDto>>(responseDtos, "Ok");
        }

        public async Task<ApiResponse<IEnumerable<ReviewResponseDto>>> GetReviewsByBookingAsync(int bookingId)
        {
            var repo = _unitOfWork.GetReposatory<Review, int>();
            var spec = new BaseSpecification<Review>(r => r.BookingId == bookingId);
            var reviews = await repo.GetAllWithSpecAsync(spec);

            var responseDtos = reviews.Select(MapToResponseDto).ToList();
            return new ApiResponse<IEnumerable<ReviewResponseDto>>(responseDtos, "Ok");
        }

        private ReviewResponseDto MapToResponseDto(Review review)
        {
            return new ReviewResponseDto
            {
                Id = review.Id,
                BookingId = review.BookingId,
                ReviewerId = review.ReviewerId,
                RevieweeId = review.RevieweeId,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt
            };
        }
    }
}
