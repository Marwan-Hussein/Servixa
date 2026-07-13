using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Servixa.Abstractions.Interfaces;
using Servixa.Shared.DTOs.Specialty;
using Servixa.Domain.Contracts.UnitOfWorkPattern;
using Servixa.Shared.Commen.Responses;
using Servixa.Domain.Models.SpecialtyEntity;

namespace Servixa.Services.Implementations
{
    public class SpecialtyService : ISpecialtyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SpecialtyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<IEnumerable<SpecialtyResponseDto>>> GetAllAsync()
        {
            var repo = _unitOfWork.GetReposatory<Specialty, int>();
            var specialties = await repo.GetAllAsync();

            var responseDtos = specialties.Select(MapToResponseDto).ToList();
            return new ApiResponse<IEnumerable<SpecialtyResponseDto>>(responseDtos, "Ok");
        }

        public async Task<ApiResponse<SpecialtyResponseDto>> GetByIdAsync(int id)
        {
            var repo = _unitOfWork.GetReposatory<Specialty, int>();
            var specialty = await repo.GetByIdAsync(id);

            if (specialty == null)
            {
                return new ApiResponse<SpecialtyResponseDto>(null!, "Not Found", 404);
            }

            return new ApiResponse<SpecialtyResponseDto>(MapToResponseDto(specialty), "Ok");
        }

        public async Task<ApiResponse<SpecialtyResponseDto>> CreateAsync(CreateSpecialtyDto dto)
        {
            var repo = _unitOfWork.GetReposatory<Specialty, int>();
            var specialty = new Specialty
            {
                Name = dto.Name ?? string.Empty,
                Description = dto.Description
            };

            await repo.AddAsync(specialty);
            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse<SpecialtyResponseDto>(MapToResponseDto(specialty), "Created");
        }

        public async Task<ApiResponse<SpecialtyResponseDto>> UpdateAsync(int id, UpdateSpecialtyDto dto)
        {
            var repo = _unitOfWork.GetReposatory<Specialty, int>();
            var specialty = await repo.GetByIdAsync(id);

            if (specialty == null)
            {
                return new ApiResponse<SpecialtyResponseDto>(null!, "Not Found", 404);
            }

            specialty.Name = dto.Name ?? string.Empty;
            specialty.Description = dto.Description;

            repo.UpdateAsync(specialty);
            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse<SpecialtyResponseDto>(MapToResponseDto(specialty), "Ok");
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            var repo = _unitOfWork.GetReposatory<Specialty, int>();
            var specialty = await repo.GetByIdAsync(id);

            if (specialty == null)
            {
                return new ApiResponse<bool>(false, "Not Found", 404);
            }

            repo.DeleteAsync(specialty);
            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse<bool>(true, "Deleted");
        }

        private SpecialtyResponseDto MapToResponseDto(Specialty specialty)
        {
            return new SpecialtyResponseDto
            {
                Id = specialty.Id,
                Name = specialty.Name,
                Description = specialty.Description
            };
        }
    }
}
