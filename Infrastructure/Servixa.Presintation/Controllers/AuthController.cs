using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
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
        private readonly IExternalAuthService _externalAuthService;
        private readonly IOtpService _otpService;

        public AuthController(IAuthService authService, IExternalAuthService externalAuthService, IOtpService otpService)
        {
            _authService = authService;
            _externalAuthService = externalAuthService;
            _otpService = otpService;
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

        [HttpGet("google-login")]
        public IActionResult GoogleLogin([FromQuery] string? requestedRole = null, [FromQuery] string? returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(GoogleCallback), "Auth", new { requestedRole, returnUrl });
            var properties = new AuthenticationProperties
            {
                RedirectUri = redirectUrl
            };

            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback([FromQuery] string? requestedRole = null, [FromQuery] string? returnUrl = null)
        {
            var result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
            if (!result.Succeeded || result.Principal == null)
            {
                return Unauthorized(new { Message = "Google authentication failed." });
            }

            var providerKey = result.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = result.Principal.FindFirstValue(ClaimTypes.Email);
            var name = result.Principal.FindFirstValue(ClaimTypes.Name);

            if (string.IsNullOrWhiteSpace(providerKey) || string.IsNullOrWhiteSpace(email))
            {
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
                return BadRequest(new { Message = "Google did not return the required profile information." });
            }

            var externalAuthResult = await _externalAuthService.ProcessExternalLoginAsync(new ExternalAuthDto
            {
                Provider = GoogleDefaults.AuthenticationScheme,
                ProviderKey = providerKey,
                Email = email,
                Name = name,
                RequestedRole = requestedRole
            });

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            if (!externalAuthResult.IsSuccess)
            {
                return BadRequest(externalAuthResult);
            }

            return Ok(externalAuthResult);
        }

        [HttpPost("verify-email-otp")]
        public async Task<IActionResult> VerifyEmailOtp([FromBody] VerifyOtpDto dto)
        {
            await _otpService.VerifyRegistrationOtpAsync(dto.Email, dto.Code);
            return Ok(new { Message = "Email verified successfully." });
        }
    }
}
