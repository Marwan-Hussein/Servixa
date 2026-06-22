using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Servixa.Abstractions.Interfaces;
using Servixa.Shared.DTOs.Auth;

namespace Servixa.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register-client")]
        public async Task<IActionResult> RegisterClient([FromBody] RegisterClientDto dto)
        {
            var result = await _authService.RegisterClientAsync(dto);
            return Ok(result);
        }

        [HttpPost("register-worker")]
        public async Task<IActionResult> RegisterWorker([FromForm] RegisterWorkerDto dto)
        {
            var result = await _authService.RegisterWorkerAsync(dto);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            return Ok(result);
        }
    }
}
