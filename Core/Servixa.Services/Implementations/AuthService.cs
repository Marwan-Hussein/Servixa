using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Servixa.Shared.DTOs.Auth;
using Servixa.Abstractions.Interfaces;
using Servixa.Domain.Models.Users;
using Servixa.Shared.Commen.Responses;

namespace Servixa.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly IConfiguration _config;
        private readonly IFileService _fileService;
        private readonly IOtpService _otpService;

        public AuthService(
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole<int>> roleManager,
            IConfiguration config,
            IFileService fileService,
            IOtpService otpService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
            _fileService = fileService;
            _otpService = otpService;
        }

        public async Task<AuthResponseDto> RegisterClientAsync(RegisterClientDto dto)
        {
            var userExists = await _userManager.FindByEmailAsync(dto.Email);
            if (userExists != null)
                throw new Exception("User already exists!");

            var client = new Client
            {
                Email = dto.Email,
                UserName = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                DefaultAddress = dto.DefaultAddress,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                EmailConfirmed = false
            };

            var result = await _userManager.CreateAsync(client, dto.Password);
            if (!result.Succeeded)
                throw new Exception("User creation failed!");

            await _userManager.AddToRoleAsync(client, "Client");
            await _otpService.SendRegistrationOtpAsync(client);

            return await GenerateAuthResponse(client);
        }

        public async Task<AuthResponseDto> RegisterWorkerAsync(RegisterWorkerDto dto)
        {
            var userExists = await _userManager.FindByEmailAsync(dto.Email);
            if (userExists != null)
                throw new Exception("User already exists!");

            string nationalIdFrontUrl = string.Empty;
            string nationalIdBackUrl = string.Empty;
            string profilePicUrl = string.Empty;

            if (dto.NationalIdFront != null)
            {
                var res = await _fileService.UploadFileAsync(dto.NationalIdFront, "worker_docs");
                if (res.IsSuccess) nationalIdFrontUrl = res.Data!;
            }
            if (dto.NationalIdBack != null)
            {
                var res = await _fileService.UploadFileAsync(dto.NationalIdBack, "worker_docs");
                if (res.IsSuccess) nationalIdBackUrl = res.Data!;
            }
            if (dto.ProfilePicture != null)
            {
                var res = await _fileService.UploadFileAsync(dto.ProfilePicture, "profiles");
                if (res.IsSuccess) profilePicUrl = res.Data!;
            }

            var worker = new Worker
            {
                Email = dto.Email,
                UserName = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                NationalIdFrontUrl = nationalIdFrontUrl,
                NationalIdBackUrl = nationalIdBackUrl,
                ProfilePictureUrl = profilePicUrl,
                EmailConfirmed = false,
                IsVerified = false // Pending admin approval
            };

            var result = await _userManager.CreateAsync(worker, dto.Password);
            if (!result.Succeeded)
                throw new Exception("Worker creation failed!");

            await _userManager.AddToRoleAsync(worker, "Worker");
            await _otpService.SendRegistrationOtpAsync(worker);

            return await GenerateAuthResponse(worker);
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                throw new Exception("Invalid email or password");

            var result = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!result)
                throw new Exception("Invalid email or password");

            return await GenerateAuthResponse(user);
        }

        public async Task ChangePasswordAsync(int userId, ChangePasswordDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new Exception("User not found");

            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (!result.Succeeded)
                throw new Exception("Failed to change password");
        }

        private async Task<AuthResponseDto> GenerateAuthResponse(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                expires: DateTime.Now.AddDays(Convert.ToDouble(_config["Jwt:DurationInDays"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new AuthResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                Roles = userRoles
            };
        }
    }
}
