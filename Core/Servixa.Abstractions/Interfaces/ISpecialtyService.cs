using System.Collections.Generic;
using System.Threading.Tasks;
using Servixa.Shared.DTOs.Specialty;
using Servixa.Shared.Commen.Responses;

namespace Servixa.Abstractions.Interfaces
{
    public interface ISpecialtyService
    {
        Task<ApiResponse<IEnumerable<SpecialtyResponseDto>>> GetAllAsync();
        Task<ApiResponse<SpecialtyResponseDto>> GetByIdAsync(int id);
        Task<ApiResponse<SpecialtyResponseDto>> CreateAsync(CreateSpecialtyDto dto);
        Task<ApiResponse<SpecialtyResponseDto>> UpdateAsync(int id, UpdateSpecialtyDto dto);
        Task<ApiResponse<bool>> DeleteAsync(int id);
    }
}
