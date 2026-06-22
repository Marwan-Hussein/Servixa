using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Servixa.Abstractions.Interfaces;
using Servixa.Shared.DTOs.Worker;
using Servixa.Domain.Contracts.UnitOfWorkPattern;
using Servixa.Shared.Commen.Responses;
using Servixa.Domain.Models.Users;
using Servixa.Domain.Models;
using Servixa.Domain.Specifications;

namespace Servixa.Services.Implementations
{
    public class WorkerService : IWorkerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WorkerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<WorkerResponseDto>> GetWorkerProfileAsync(int workerId)
        {
            var repo = _unitOfWork.GetReposatory<Worker, int>();
            var spec = new WorkerWithDetailsSpecification(workerId);
            var worker = await repo.GetEntityWithSpecAsync(spec);

            if (worker == null)
            {
                return new ApiResponse<WorkerResponseDto>(null!, "Not Found", 404);
            }

            var responseDto = MapToResponseDto(worker);
            return new ApiResponse<WorkerResponseDto>(responseDto, "Ok");
        }

        public async Task<ApiResponse<IEnumerable<WorkerResponseDto>>> GetWorkersBySpecialtyAsync(int specialtyId)
        {
            var repo = _unitOfWork.GetReposatory<Worker, int>();
            var spec = new WorkerWithDetailsSpecification(specialtyId, true);
            var workers = await repo.GetAllWithSpecAsync(spec);

            var responseDtos = workers.Select(MapToResponseDto).ToList();
            return new ApiResponse<IEnumerable<WorkerResponseDto>>(responseDtos, "Ok");
        }

        public async Task<ApiResponse<IEnumerable<WorkerResponseDto>>> GetNearbyWorkersAsync(decimal lat, decimal lng, double radiusInKm)
        {
            var repo = _unitOfWork.GetReposatory<Worker, int>();
            var spec = new WorkerWithDetailsSpecification();
            var workers = await repo.GetAllWithSpecAsync(spec);

            var nearbyWorkers = workers.Where(w =>
            {
                var distance = CalculateDistance((double)lat, (double)lng, w.Latitude, w.Longitude);
                return distance <= radiusInKm;
            }).Select(MapToResponseDto).ToList();

            return new ApiResponse<IEnumerable<WorkerResponseDto>>(nearbyWorkers, "Ok");
        }

        public async Task<ApiResponse<bool>> AddWorkerTaskAsync(int workerId, WorkerTaskDto dto)
        {
            var repo = _unitOfWork.GetReposatory<WorkerTask, int>();
            var workerTask = new WorkerTask
            {
                WorkerId = workerId,
                TaskId = dto.TaskId,
                CustomPrice = dto.CustomPrice
            };

            await repo.AddAsync(workerTask);
            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse<bool>(true, "Added");
        }

        public async Task<ApiResponse<bool>> RemoveWorkerTaskAsync(int workerId, int taskId)
        {
            var repo = _unitOfWork.GetReposatory<WorkerTask, int>();
            var workerTasks = await repo.GetAllAsync();
            var workerTask = workerTasks.FirstOrDefault(wt => wt.WorkerId == workerId && wt.TaskId == taskId);

            if (workerTask == null)
            {
                return new ApiResponse<bool>(false, "Not Found", 404);
            }

            repo.DeleteAsync(workerTask);
            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse<bool>(true, "Removed");
        }

        public async Task<ApiResponse<bool>> ToggleAvailabilityAsync(int workerId)
        {
            var repo = _unitOfWork.GetReposatory<Worker, int>();
            var worker = await repo.GetByIdAsync(workerId);

            if (worker == null)
            {
                return new ApiResponse<bool>(false, "Not Found", 404);
            }

            worker.IsAvailable = !worker.IsAvailable;
            repo.UpdateAsync(worker);
            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse<bool>(true, "Toggled");
        }

        private WorkerResponseDto MapToResponseDto(Worker worker)
        {
            return new WorkerResponseDto
            {
                Id = worker.Id,
                FirstName = worker.FirstName,
                LastName = worker.LastName,
                SpecialtyId = worker.WorkerTasks?.FirstOrDefault()?.Task?.SpecialtyId ?? 0,
                AverageRating = worker.AverageRating,
                IsAvailable = worker.IsAvailable
            };
        }

        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var radius = 6371; 
            return radius * c;
        }

        private double ToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }
    }
}
