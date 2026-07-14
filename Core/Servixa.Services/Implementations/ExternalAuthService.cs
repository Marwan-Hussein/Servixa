using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Servixa.Abstractions.Interfaces;
using Servixa.Domain.Models.Users;
using Servixa.Shared.DTOs.Auth;

namespace Servixa.Services.Implementations
{
    public class ExternalAuthService(
        UserManager<ApplicationUser> userManager,
        IConfiguration config) : IExternalAuthService
    {
        public async Task<ExternalAuthResponseDto> ProcessExternalLoginAsync(ExternalAuthDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Provider) ||
                string.IsNullOrWhiteSpace(dto.ProviderKey) ||
                string.IsNullOrWhiteSpace(dto.Email))
            {
                return new ExternalAuthResponseDto
                {
                    IsSuccess = false,
                    Message = "External login data is incomplete."
                };
            }

            var user = await userManager.FindByLoginAsync(dto.Provider, dto.ProviderKey);

            if (user == null)
            {
                user = await userManager.FindByEmailAsync(dto.Email);

                if (user != null)
                {
                    var linkResult = await userManager.AddLoginAsync(user, new UserLoginInfo(dto.Provider, dto.ProviderKey, dto.Provider));
                    if (!linkResult.Succeeded)
                    {
                        return new ExternalAuthResponseDto
                        {
                            IsSuccess = false,
                            Message = "Failed to link Google login to the existing account."
                        };
                    }
                }
                else
                {
                    var roleToAssign = string.Equals(dto.RequestedRole, "Worker", StringComparison.OrdinalIgnoreCase)
                        ? "Worker"
                        : "Client";

                    user = roleToAssign == "Worker"
                        ? new Worker { IsVerified = false }
                        : new Client();

                    ApplyGoogleProfile(user, dto);

                    var createResult = await userManager.CreateAsync(user);
                    if (!createResult.Succeeded)
                    {
                        var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                        return new ExternalAuthResponseDto { IsSuccess = false, Message = $"User registration failed: {errors}" };
                    }

                    var linkResult = await userManager.AddLoginAsync(user, new UserLoginInfo(dto.Provider, dto.ProviderKey, dto.Provider));
                    if (!linkResult.Succeeded)
                    {
                        await userManager.DeleteAsync(user);
                        return new ExternalAuthResponseDto { IsSuccess = false, Message = "Failed to link Google login." };
                    }

                    var roleResult = await userManager.AddToRoleAsync(user, roleToAssign);
                    if (!roleResult.Succeeded)
                    {
                        await userManager.DeleteAsync(user);
                        var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                        return new ExternalAuthResponseDto { IsSuccess = false, Message = $"Failed to assign role: {errors}" };
                    }
                }
            }

            return new ExternalAuthResponseDto
            {
                IsSuccess = true,
                Message = "Authentication successful",
                User = await GenerateAuthResponse(user)
            };
        }

        private static void ApplyGoogleProfile(ApplicationUser user, ExternalAuthDto dto)
        {
            var nameParts = (dto.Name ?? dto.Email).Split(' ', 2, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            user.UserName = dto.Email;
            user.Email = dto.Email;
            user.EmailConfirmed = true;
            user.FirstName = nameParts.ElementAtOrDefault(0) ?? dto.Email;
            user.LastName = nameParts.ElementAtOrDefault(1) ?? string.Empty;
            user.IsDeleted = false;
        }

        private async Task<AuthResponseDto> GenerateAuthResponse(ApplicationUser user)
        {
            var userRoles = await userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName ?? string.Empty),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                expires: DateTime.Now.AddDays(Convert.ToDouble(config["Jwt:DurationInDays"])),
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
