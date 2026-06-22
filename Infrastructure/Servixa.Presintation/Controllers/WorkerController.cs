using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Servixa.Abstractions.Interfaces;
using Servixa.Shared.DTOs.Worker;

namespace Servixa.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkerController : ControllerBase
    {
        private readonly IWorkerService _workerService;

        public WorkerController(IWorkerService workerService)
        {
            _workerService = workerService;
        }

        [HttpGet("{workerId}")]
        public async Task<IActionResult> GetWorkerProfile(int workerId)
        {
            var result = await _workerService.GetWorkerProfileAsync(workerId);
            return Ok(result);
        }

        [HttpGet("specialty/{specialtyId}")]
        public async Task<IActionResult> GetWorkersBySpecialty(int specialtyId)
        {
            var result = await _workerService.GetWorkersBySpecialtyAsync(specialtyId);
            return Ok(result);
        }

        [HttpGet("nearby")]
        public async Task<IActionResult> GetNearbyWorkers([FromQuery] decimal lat, [FromQuery] decimal lng, [FromQuery] double radiusInKm)
        {
            var result = await _workerService.GetNearbyWorkersAsync(lat, lng, radiusInKm);
            return Ok(result);
        }

        [HttpPost("{workerId}/tasks")]
        public async Task<IActionResult> AddWorkerTask(int workerId, [FromBody] WorkerTaskDto dto)
        {
            var result = await _workerService.AddWorkerTaskAsync(workerId, dto);
            return Ok(result);
        }

        [HttpDelete("{workerId}/tasks/{taskId}")]
        public async Task<IActionResult> RemoveWorkerTask(int workerId, int taskId)
        {
            var result = await _workerService.RemoveWorkerTaskAsync(workerId, taskId);
            return Ok(result);
        }

        [HttpPut("{workerId}/toggle-availability")]
        public async Task<IActionResult> ToggleAvailability(int workerId)
        {
            var result = await _workerService.ToggleAvailabilityAsync(workerId);
            return Ok(result);
        }
    }
}
