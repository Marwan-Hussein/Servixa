using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Servixa.Abstractions.Interfaces;
using Servixa.Shared.DTOs.Client;
using Servixa.Shared.DTOs.Booking;
using Servixa.Shared.DTOs.Review;
using Servixa.Domain.Contracts.UnitOfWorkPattern;
using Servixa.Shared.Commen.Responses;
using Servixa.Domain.Models.Users;
using Servixa.Domain.Models.BookingEntity;
using Servixa.Domain.Models.ReviewEntity;
using Servixa.Domain.Specifications;
using Servixa.Domain.Contracts;
using Servixa.Shared.Enums;

namespace Servixa.Services.Implementations
{
    public class ClientService : IClientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;
        private readonly IBookingService _bookingService;

        public ClientService(IUnitOfWork unitOfWork, IFileService fileService, IBookingService bookingService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
            _bookingService = bookingService;
        }

        // ── Profile ────────────────────────────────────────────────────────────

        public async Task<ApiResponse<ClientResponseDto>> GetClientProfileAsync(int clientId)
        {
            var repo = _unitOfWork.GetReposatory<Client, int>();
            var spec = new ClientWithDetailsSpecification(clientId);
            var client = await repo.GetEntityWithSpecAsync(spec);

            if (client == null)
                return new ApiResponse<ClientResponseDto>("Client not found", 404);

            return new ApiResponse<ClientResponseDto>(MapToResponseDto(client), "Ok");
        }

        public async Task<ApiResponse<ClientResponseDto>> UpdateClientProfileAsync(int clientId, UpdateClientProfileDto dto)
        {
            var repo = _unitOfWork.GetReposatory<Client, int>();
            var spec = new ClientWithDetailsSpecification(clientId);
            var client = await repo.GetEntityWithSpecAsync(spec);

            if (client == null)
                return new ApiResponse<ClientResponseDto>("Client not found", 404);

            client.FirstName = dto.FirstName;
            client.LastName = dto.LastName;
            client.PhoneNumber = dto.PhoneNumber;
            client.DefaultAddress = dto.DefaultAddress;
            client.Latitude = dto.Latitude;
            client.Longitude = dto.Longitude;

            if (dto.ProfilePicture != null)
            {
                // Remove old picture if present
                if (!string.IsNullOrEmpty(client.ProfilePictureUrl))
                    await _fileService.DeleteFileAsync(client.ProfilePictureUrl);

                var uploadResult = await _fileService.UploadFileAsync(dto.ProfilePicture, "profiles");
                if (uploadResult.IsSuccess)
                    client.ProfilePictureUrl = uploadResult.Data;
            }

            repo.UpdateAsync(client);
            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse<ClientResponseDto>(MapToResponseDto(client), "Profile updated successfully");
        }

        // ── Bookings ────────────────────────────────────────────────────────────

        public async Task<ApiResponse<BookingResponseDto>> BookTaskAsync(int clientId, CreateBookingDto dto)
        {
            // Delegate creation to the shared BookingService (which validates worker & task)
            return await _bookingService.CreateBookingAsync(clientId, dto);
        }

        public async Task<ApiResponse<IEnumerable<BookingResponseDto>>> GetMyBookingsAsync(int clientId)
        {
            return await _bookingService.GetClientBookingsAsync(clientId);
        }

        public async Task<ApiResponse<BookingResponseDto>> GetBookingDetailsAsync(int clientId, int bookingId)
        {
            var result = await _bookingService.GetBookingByIdAsync(bookingId);

            // Ensure the booking belongs to this client
            if (result.IsSuccess && result.Data!.ClientId != clientId)
                return new ApiResponse<BookingResponseDto>("You are not authorised to view this booking", 403);

            return result;
        }

        public async Task<ApiResponse<bool>> CancelMyBookingAsync(int clientId, int bookingId)
        {
            // First verify ownership
            var bookingResult = await _bookingService.GetBookingByIdAsync(bookingId);

            if (!bookingResult.IsSuccess)
                return new ApiResponse<bool>(bookingResult.Message, bookingResult.StatusCode);

            if (bookingResult.Data!.ClientId != clientId)
                return new ApiResponse<bool>("You are not authorised to cancel this booking", 403);

            if (bookingResult.Data.Status == BookingStatus.Completed.ToString())
                return new ApiResponse<bool>("Cannot cancel a completed booking", 400);

            return await _bookingService.CancelBookingAsync(bookingId);
        }

        // ── Reviews ─────────────────────────────────────────────────────────────

        public async Task<ApiResponse<ReviewResponseDto>> LeaveReviewAsync(int clientId, CreateReviewDto dto)
        {
            // Verify the booking belongs to this client and is completed
            var bookingRepo = _unitOfWork.GetReposatory<Booking, int>();
            var spec = new BookingWithDetailsSpecification(dto.BookingId);
            var booking = await bookingRepo.GetEntityWithSpecAsync(spec);

            if (booking == null)
                return new ApiResponse<ReviewResponseDto>("Booking not found", 404);

            if (booking.ClientId != clientId)
                return new ApiResponse<ReviewResponseDto>("You can only review your own bookings", 403);

            if (booking.Status != BookingStatus.Completed)
                return new ApiResponse<ReviewResponseDto>("You can only review a completed booking", 400);

            // Check if a review already exists for this booking by this client
            var reviewRepo = _unitOfWork.GetReposatory<Review, int>();
            var existingReviews = await reviewRepo.GetAllAsync();
            var alreadyReviewed = existingReviews.Any(r => r.BookingId == dto.BookingId && r.ReviewerId == clientId);

            if (alreadyReviewed)
                return new ApiResponse<ReviewResponseDto>("You have already reviewed this booking", 400);

            var review = new Review
            {
                BookingId = dto.BookingId,
                ReviewerId = clientId,
                RevieweeId = booking.WorkerId,
                Rating = dto.Rating,
                Comment = dto.Comment
            };

            await reviewRepo.AddAsync(review);
            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse<ReviewResponseDto>(MapReviewToDto(review), "Review submitted successfully");
        }

        public async Task<ApiResponse<IEnumerable<ReviewResponseDto>>> GetMyReviewsAsync(int clientId)
        {
            var reviewRepo = _unitOfWork.GetReposatory<Review, int>();
            var spec = new BaseSpecification<Review>(r => r.ReviewerId == clientId);
            var reviews = await reviewRepo.GetAllWithSpecAsync(spec);

            var dtos = reviews.Select(MapReviewToDto).ToList();
            return new ApiResponse<IEnumerable<ReviewResponseDto>>(dtos, "Ok");
        }

        // ── Mapping helpers ─────────────────────────────────────────────────────

        private static ClientResponseDto MapToResponseDto(Client client) => new ClientResponseDto
        {
            Id = client.Id,
            FirstName = client.FirstName,
            LastName = client.LastName,
            Email = client.Email,
            PhoneNumber = client.PhoneNumber,
            DefaultAddress = client.DefaultAddress,
            Latitude = client.Latitude,
            Longitude = client.Longitude,
            ProfilePictureUrl = client.ProfilePictureUrl
        };

        private static ReviewResponseDto MapReviewToDto(Review review) => new ReviewResponseDto
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
