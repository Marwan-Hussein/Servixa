using System.Collections.Generic;
using System.Threading.Tasks;
using Servixa.Shared.DTOs.Task;
using Servixa.Shared.Commen.Responses;

namespace Servixa.Abstractions.Interfaces
{
    public interface ITaskService
    {
        Task<ApiResponse<IEnumerable<TaskResponseDto>>> GetAllAsync();
        Task<ApiResponse<IEnumerable<TaskResponseDto>>> GetBySpecialtyAsync(int specialtyId);
        Task<ApiResponse<TaskResponseDto>> GetByIdAsync(int id);
        Task<ApiResponse<TaskResponseDto>> CreateAsync(CreateTaskDto dto);
        Task<ApiResponse<TaskResponseDto>> UpdateAsync(int id, UpdateTaskDto dto);
        Task<ApiResponse<bool>> DeleteAsync(int id);
    }
}
