using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Servixa.Abstractions.Interfaces;
using Servixa.Shared.DTOs.Admin;

namespace Servixa.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("pending-workers")]
        public async Task<IActionResult> GetPendingWorkers()
        {
            var result = await _adminService.GetPendingWorkersAsync();
            return Ok(result);
        }

        [HttpPut("workers/{workerId}/approve")]
        public async Task<IActionResult> ApproveWorker(int workerId)
        {
            var result = await _adminService.ApproveWorkerAsync(workerId);
            return Ok(result);
        }

        [HttpPut("workers/{workerId}/reject")]
        public async Task<IActionResult> RejectWorker(int workerId)
        {
            var result = await _adminService.RejectWorkerAsync(workerId);
            return Ok(result);
        }

        [HttpPut("workers/{workerId}/block")]
        public async Task<IActionResult> BlockWorker(int workerId)
        {
            var result = await _adminService.BlockWorkerAsync(workerId);
            return Ok(result);
        }

        [HttpGet("dashboard-stats")]
        public async Task<IActionResult> GetDashboardStats()
        {
            var result = await _adminService.GetDashboardStatsAsync();
            return Ok(result);
        }
    }
}
