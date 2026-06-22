using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Servixa.Abstractions.Interfaces;
using Servixa.Shared.DTOs.Booking;
using Servixa.Domain.Contracts.UnitOfWorkPattern;
using Servixa.Shared.Commen.Responses;
using Servixa.Domain.Models.BookingEntity;
using Servixa.Shared.Enums;
using Servixa.Domain.Specifications;

namespace Servixa.Services.Implementations
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<BookingResponseDto>> CreateBookingAsync(CreateBookingDto dto)
        {
            var booking = new Booking
            {
                ClientId = 1,
                WorkerId = dto.WorkerId,
                TaskId = dto.TaskId,
                ScheduledDate = dto.ScheduledDate,
                LocationLatitude = (double)dto.LocationLat,
                LocationLongitude = (double)dto.LocationLng,
                LocationAddress = dto.Address,
                Status = BookingStatus.Pending,
                FinalCost = 0
            };

            var repo = _unitOfWork.GetReposatory<Booking, int>();
            await repo.AddAsync(booking);
            await _unitOfWork.SaveChangesAsync();

            var responseDto = MapToResponseDto(booking);
            return new ApiResponse<BookingResponseDto>(responseDto, "Created");
        }

        public async Task<ApiResponse<BookingResponseDto>> GetBookingByIdAsync(int id)
        {
            var repo = _unitOfWork.GetReposatory<Booking, int>();
            var spec = new BookingWithDetailsSpecification(id);
            var booking = await repo.GetEntityWithSpecAsync(spec);

            if (booking == null)
            {
                return new ApiResponse<BookingResponseDto>(null!, "Not Found", 404);
            }

            var responseDto = MapToResponseDto(booking);
            return new ApiResponse<BookingResponseDto>(responseDto, "Ok");
        }

        public async Task<ApiResponse<IEnumerable<BookingResponseDto>>> GetClientBookingsAsync(int clientId)
        {
            var repo = _unitOfWork.GetReposatory<Booking, int>();
            var spec = new BookingWithDetailsSpecification(clientId.ToString(), true);
            var bookings = await repo.GetAllWithSpecAsync(spec);

            var responseDtos = bookings.Select(MapToResponseDto).ToList();
            return new ApiResponse<IEnumerable<BookingResponseDto>>(responseDtos, "Ok");
        }

        public async Task<ApiResponse<IEnumerable<BookingResponseDto>>> GetWorkerBookingsAsync(int workerId)
        {
            var repo = _unitOfWork.GetReposatory<Booking, int>();
            var spec = new BookingWithDetailsSpecification(workerId.ToString(), false);
            var bookings = await repo.GetAllWithSpecAsync(spec);

            var responseDtos = bookings.Select(MapToResponseDto).ToList();
            return new ApiResponse<IEnumerable<BookingResponseDto>>(responseDtos, "Ok");
        }

        public async Task<ApiResponse<BookingResponseDto>> UpdateBookingStatusAsync(int id, UpdateBookingStatusDto dto)
        {
            var repo = _unitOfWork.GetReposatory<Booking, int>();
            var booking = await repo.GetByIdAsync(id);

            if (booking == null)
            {
                return new ApiResponse<BookingResponseDto>(null!, "Not Found", 404);
            }

            booking.Status = dto.Status;
            repo.UpdateAsync(booking);
            await _unitOfWork.SaveChangesAsync();

            var responseDto = MapToResponseDto(booking);
            return new ApiResponse<BookingResponseDto>(responseDto, "Ok");
        }

        public async Task<ApiResponse<bool>> CancelBookingAsync(int id)
        {
            var repo = _unitOfWork.GetReposatory<Booking, int>();
            var booking = await repo.GetByIdAsync(id);

            if (booking == null)
            {
                return new ApiResponse<bool>(false, "Not Found", 404);
            }

            booking.Status = BookingStatus.Cancelled;
            repo.UpdateAsync(booking);
            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse<bool>(true, "Cancelled");
        }

        private BookingResponseDto MapToResponseDto(Booking booking)
        {
            return new BookingResponseDto
            {
                Id = booking.Id,
                ClientId = booking.ClientId,
                WorkerId = booking.WorkerId,
                TaskId = booking.TaskId,
                Status = booking.Status.ToString(),
                ScheduledDate = booking.ScheduledDate,
                TotalPrice = booking.FinalCost,
                Address = booking.LocationAddress,
                CreatedAt = booking.CreatedAt
            };
        }
    }
}
