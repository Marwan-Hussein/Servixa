using System.Collections.Generic;
using System.Threading.Tasks;
using Servixa.Shared.DTOs.Admin;
using Servixa.Shared.DTOs.Worker;
using Servixa.Shared.Commen.Responses;

namespace Servixa.Abstractions.Interfaces
{
    public interface IAdminService
    {
        Task<ApiResponse<IEnumerable<WorkerDetailResponseDto>>> GetPendingWorkersAsync();
        Task<ApiResponse<bool>> ApproveWorkerAsync(int workerId);
        Task<ApiResponse<bool>> RejectWorkerAsync(int workerId);
        Task<ApiResponse<bool>> BlockWorkerAsync(int workerId);
        Task<ApiResponse<DashboardStatsResponseDto>> GetDashboardStatsAsync();
    }
}
