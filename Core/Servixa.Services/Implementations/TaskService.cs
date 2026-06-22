using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Servixa.Abstractions.Interfaces;
using Servixa.Shared.DTOs.Task;
using Servixa.Domain.Contracts.UnitOfWorkPattern;
using Servixa.Shared.Commen.Responses;
using Servixa.Domain.Models.TaskEntity;
using Servixa.Domain.Specifications;

namespace Servixa.Services.Implementations
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TaskService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<IEnumerable<TaskResponseDto>>> GetAllAsync()
        {
            var repo = _unitOfWork.GetReposatory<Servixa.Domain.Models.TaskEntity.Task, int>();
            var spec = new TaskWithSpecialtySpecification();
            var tasks = await repo.GetAllWithSpecAsync(spec);

            var responseDtos = tasks.Select(MapToResponseDto).ToList();
            return new ApiResponse<IEnumerable<TaskResponseDto>>(responseDtos, "Ok");
        }

        public async Task<ApiResponse<IEnumerable<TaskResponseDto>>> GetBySpecialtyAsync(int specialtyId)
        {
            var repo = _unitOfWork.GetReposatory<Servixa.Domain.Models.TaskEntity.Task, int>();
            var spec = new TaskWithSpecialtySpecification(specialtyId, true);
            var tasks = await repo.GetAllWithSpecAsync(spec);

            var responseDtos = tasks.Select(MapToResponseDto).ToList();
            return new ApiResponse<IEnumerable<TaskResponseDto>>(responseDtos, "Ok");
        }

        public async Task<ApiResponse<TaskResponseDto>> GetByIdAsync(int id)
        {
            var repo = _unitOfWork.GetReposatory<Servixa.Domain.Models.TaskEntity.Task, int>();
            var spec = new TaskWithSpecialtySpecification(id);
            var task = await repo.GetEntityWithSpecAsync(spec);

            if (task == null)
            {
                return new ApiResponse<TaskResponseDto>(null!, "Not Found", 404);
            }

            return new ApiResponse<TaskResponseDto>(MapToResponseDto(task), "Ok");
        }

        public async Task<ApiResponse<TaskResponseDto>> CreateAsync(CreateTaskDto dto)
        {
            var repo = _unitOfWork.GetReposatory<Servixa.Domain.Models.TaskEntity.Task, int>();
            var task = new Servixa.Domain.Models.TaskEntity.Task
            {
                Name = dto.Name ?? string.Empty,
                AvgCost = dto.AvgCost,
                AvgTime = DateTime.MinValue.AddMinutes(dto.AvgTime),
                SpecialtyId = dto.SpecialtyId
            };

            await repo.AddAsync(task);
            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse<TaskResponseDto>(MapToResponseDto(task), "Created");
        }

        public async Task<ApiResponse<TaskResponseDto>> UpdateAsync(int id, UpdateTaskDto dto)
        {
            var repo = _unitOfWork.GetReposatory<Servixa.Domain.Models.TaskEntity.Task, int>();
            var task = await repo.GetByIdAsync(id);

            if (task == null)
            {
                return new ApiResponse<TaskResponseDto>(null!, "Not Found", 404);
            }

            task.Name = dto.Name ?? string.Empty;
            task.AvgCost = dto.AvgCost;
            task.AvgTime = DateTime.MinValue.AddMinutes(dto.AvgTime);
            task.SpecialtyId = dto.SpecialtyId;

            repo.UpdateAsync(task);
            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse<TaskResponseDto>(MapToResponseDto(task), "Ok");
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            var repo = _unitOfWork.GetReposatory<Servixa.Domain.Models.TaskEntity.Task, int>();
            var task = await repo.GetByIdAsync(id);

            if (task == null)
            {
                return new ApiResponse<bool>(false, "Not Found", 404);
            }

            repo.DeleteAsync(task);
            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse<bool>(true, "Deleted");
        }

        private TaskResponseDto MapToResponseDto(Servixa.Domain.Models.TaskEntity.Task task)
        {
            return new TaskResponseDto
            {
                Id = task.Id,
                Name = task.Name,
                AvgCost = task.AvgCost,
                AvgTime = (int)(task.AvgTime - DateTime.MinValue).TotalMinutes,
                SpecialtyId = task.SpecialtyId
            };
        }
    }
}
