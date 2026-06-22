using System.Collections.Generic;
using System.Threading.Tasks;
using Servixa.Shared.DTOs.Worker;
using Servixa.Shared.Commen.Responses;

namespace Servixa.Abstractions.Interfaces
{
    public interface IWorkerService
    {
        Task<ApiResponse<WorkerResponseDto>> GetWorkerProfileAsync(int workerId);
        Task<ApiResponse<IEnumerable<WorkerResponseDto>>> GetWorkersBySpecialtyAsync(int specialtyId);
        Task<ApiResponse<IEnumerable<WorkerResponseDto>>> GetNearbyWorkersAsync(decimal lat, decimal lng, double radiusInKm);
        Task<ApiResponse<bool>> AddWorkerTaskAsync(int workerId, WorkerTaskDto dto);
        Task<ApiResponse<bool>> RemoveWorkerTaskAsync(int workerId, int taskId);
        Task<ApiResponse<bool>> ToggleAvailabilityAsync(int workerId);
    }
}
