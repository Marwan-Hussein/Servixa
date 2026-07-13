using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Servixa.Abstractions.Interfaces;
using Servixa.Shared.DTOs.Admin;
using Servixa.Shared.DTOs.Worker;
using Servixa.Domain.Contracts.UnitOfWorkPattern;
using Servixa.Domain.Models.Users;
using Servixa.Shared.Commen.Responses;

namespace Servixa.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<ApiResponse<IEnumerable<WorkerDetailResponseDto>>> GetPendingWorkersAsync()
        {
            var workersRepo = _unitOfWork.GetReposatory<Worker, int>();
            var allWorkers = await workersRepo.GetAllAsync();
            var pendingWorkers = allWorkers.Where(w => !w.IsVerified).Select(w => new WorkerDetailResponseDto
            {
                Id = w.Id,
                FirstName = w.FirstName,
                LastName = w.LastName,
                Email = w.Email ?? string.Empty,
                PhoneNumber = w.PhoneNumber ?? string.Empty,
                Latitude = w.Latitude,
                Longitude = w.Longitude,
                IsVerified = w.IsVerified,
                ProfilePictureUrl = w.ProfilePictureUrl ?? string.Empty,
                NationalIdFrontUrl = w.NationalIdFrontUrl ?? string.Empty,
                NationalIdBackUrl = w.NationalIdBackUrl ?? string.Empty
            });

            return new ApiResponse<IEnumerable<WorkerDetailResponseDto>>(pendingWorkers, "Ok");
        }

        public async Task<ApiResponse<bool>> ApproveWorkerAsync(int workerId)
        {
            var workersRepo = _unitOfWork.GetReposatory<Worker, int>();
            var worker = await workersRepo.GetByIdAsync(workerId);
            if (worker == null) return new ApiResponse<bool>(false, "Worker not found", 404);

            worker.IsVerified = true;
            workersRepo.UpdateAsync(worker);
            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse<bool>(true, "Worker approved");
        }

        public async Task<ApiResponse<bool>> RejectWorkerAsync(int workerId)
        {
            var worker = await _userManager.FindByIdAsync(workerId.ToString());
            if (worker == null) return new ApiResponse<bool>(false, "Worker not found", 404);

            await _userManager.DeleteAsync(worker);
            
            return new ApiResponse<bool>(true, "Worker rejected and deleted");
        }

        public async Task<ApiResponse<bool>> BlockWorkerAsync(int workerId)
        {
            var worker = await _userManager.FindByIdAsync(workerId.ToString());
            if (worker == null) return new ApiResponse<bool>(false, "Worker not found", 404);

            var result = await _userManager.SetLockoutEndDateAsync(worker, DateTimeOffset.UtcNow.AddYears(100));
            return new ApiResponse<bool>(result.Succeeded, result.Succeeded ? "Blocked" : "Failed to block");
        }

        public async Task<ApiResponse<DashboardStatsResponseDto>> GetDashboardStatsAsync()
        {
            var bookingsRepo = _unitOfWork.GetReposatory<Domain.Models.BookingEntity.Booking, int>();
            var paymentsRepo = _unitOfWork.GetReposatory<Domain.Models.PaymentEntity.Payment, int>();

            var totalBookings = await bookingsRepo.GetAllAsync();
            var totalPayments = await paymentsRepo.GetAllAsync();
            var completedPayments = totalPayments.Where(p => p.Status == Servixa.Shared.Enums.PaymentStatus.Completed);

            var stats = new DashboardStatsResponseDto
            {
                TotalBookings = totalBookings.Count(),
                TotalRevenue = completedPayments.Sum(p => p.Amount),
                TotalUsers = _userManager.Users.Count()
            };

            return new ApiResponse<DashboardStatsResponseDto>(stats, "Ok");
        }
    }
}
