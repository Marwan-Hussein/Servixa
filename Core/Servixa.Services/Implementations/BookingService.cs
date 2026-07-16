using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Servixa.Abstractions.Interfaces;
using Servixa.Shared.DTOs.Booking;
using Servixa.Domain.Contracts.UnitOfWorkPattern;
using Servixa.Shared.Commen.Responses;
using Servixa.Domain.Models.BookingEntity;
using Servixa.Domain.Models.Users;
using Servixa.Domain.Models;
using Servixa.Shared.Enums;
using Servixa.Domain.Specifications;
using Servixa.Domain.Contracts;

namespace Servixa.Services.Implementations
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<BookingResponseDto>> CreateBookingAsync(int clientId, CreateBookingDto dto)
        {
            // Validate that the worker exists and is available
            var workerRepo = _unitOfWork.GetReposatory<Worker, int>();
            var worker = await workerRepo.GetByIdAsync(dto.WorkerId);

            if (worker == null)
                return new ApiResponse<BookingResponseDto>("Worker not found", 404);

            if (!worker.IsAvailable)
                return new ApiResponse<BookingResponseDto>("Worker is not available at this time", 400);

            // Validate the worker actually offers the requested task
            var workerTaskRepo = _unitOfWork.GetReposatory<WorkerTask, int>();
            var allWorkerTasks = await workerTaskRepo.GetAllAsync();
            var workerTask = allWorkerTasks.FirstOrDefault(wt => wt.WorkerId == dto.WorkerId && wt.TaskId == dto.TaskId);

            if (workerTask == null)
                return new ApiResponse<BookingResponseDto>("The selected worker does not offer this task", 400);

            var booking = new Booking
            {
                ClientId = clientId,
                WorkerId = dto.WorkerId,
                TaskId = dto.TaskId,
                ScheduledDate = dto.ScheduledDate,
                LocationLatitude = (double)dto.LocationLat,
                LocationLongitude = (double)dto.LocationLng,
                LocationAddress = dto.Address,
                Status = BookingStatus.Pending,
                FinalCost = workerTask.CustomPrice
            };

            var repo = _unitOfWork.GetReposatory<Booking, int>();
            await repo.AddAsync(booking);
            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse<BookingResponseDto>(MapToResponseDto(booking), "Booking created successfully");
        }

        public async Task<ApiResponse<BookingResponseDto>> GetBookingByIdAsync(int id)
        {
            var repo = _unitOfWork.GetReposatory<Booking, int>();
            var spec = new BookingWithDetailsSpecification(id);
            var booking = await repo.GetEntityWithSpecAsync(spec);

            if (booking == null)
                return new ApiResponse<BookingResponseDto>("Booking not found", 404);

            return new ApiResponse<BookingResponseDto>(MapToResponseDto(booking), "Ok");
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
                return new ApiResponse<BookingResponseDto>("Booking not found", 404);

            booking.Status = dto.Status;
            repo.UpdateAsync(booking);
            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse<BookingResponseDto>(MapToResponseDto(booking), "Booking status updated");
        }

        public async Task<ApiResponse<bool>> CancelBookingAsync(int id)
        {
            var repo = _unitOfWork.GetReposatory<Booking, int>();
            var booking = await repo.GetByIdAsync(id);

            if (booking == null)
                return new ApiResponse<bool>("Booking not found", 404);

            if (booking.Status == BookingStatus.Completed)
                return new ApiResponse<bool>("Cannot cancel a completed booking", 400);

            if (booking.Status == BookingStatus.Cancelled)
                return new ApiResponse<bool>("Booking is already cancelled", 400);

            booking.Status = BookingStatus.Cancelled;
            repo.UpdateAsync(booking);
            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse<bool>(true, "Booking cancelled successfully");
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
