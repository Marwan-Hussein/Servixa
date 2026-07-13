using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Servixa.Abstractions.Interfaces;
using Servixa.Domain.Contracts.UnitOfWorkPattern;
using Servixa.Domain.Models.OtpEntity;
using Servixa.Domain.Models.Users;
using Servixa.Domain.Specifications;

namespace Servixa.Services.Implementations
{
    public class OtpService : IOtpService
    {
        private const string RegistrationPurpose = "Registration";
        private static readonly TimeSpan OtpLifetime = TimeSpan.FromMinutes(10);

        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly UserManager<ApplicationUser> _userManager;

        public OtpService(
            IUnitOfWork unitOfWork,
            IEmailService emailService,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _userManager = userManager;
        }

        public async Task SendRegistrationOtpAsync(ApplicationUser user)
        {
            if (string.IsNullOrWhiteSpace(user.Email))
                throw new Exception("User email is required to send OTP.");

            var now = DateTime.UtcNow;
            var repo = _unitOfWork.GetReposatory<OtpCode, int>();
            var activeCodes = await repo.GetAllWithSpecAsync(new ActiveOtpCodeSpecification(user.Id, RegistrationPurpose, now));

            foreach (var activeCode in activeCodes)
            {
                activeCode.IsUsed = true;
                activeCode.UsedAt = now;
                repo.UpdateAsync(activeCode);
            }

            var code = GenerateCode();
            var otp = new OtpCode
            {
                UserId = user.Id,
                Email = user.Email,
                CodeHash = HashCode(code, user.Id, RegistrationPurpose),
                Purpose = RegistrationPurpose,
                ExpiresAt = now.Add(OtpLifetime)
            };

            await repo.AddAsync(otp);
            await _unitOfWork.SaveChangesAsync();

            var displayName = $"{user.FirstName} {user.LastName}".Trim();
            await _emailService.SendOtpEmailAsync(user.Email, displayName, code, otp.ExpiresAt);
        }

        public async Task VerifyRegistrationOtpAsync(string email, string code)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(code))
                throw new Exception("Email and OTP code are required.");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new Exception("User not found.");

            var now = DateTime.UtcNow;
            var repo = _unitOfWork.GetReposatory<OtpCode, int>();
            var activeCodes = await repo.GetAllWithSpecAsync(new ActiveOtpCodeSpecification(user.Id, RegistrationPurpose, now));
            var codeHash = HashCode(code.Trim(), user.Id, RegistrationPurpose);
            var otp = activeCodes.FirstOrDefault(o => o.CodeHash == codeHash);

            if (otp == null)
                throw new Exception("Invalid or expired OTP code.");

            otp.IsUsed = true;
            otp.UsedAt = now;
            repo.UpdateAsync(otp);

            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        private static string GenerateCode()
        {
            var value = RandomNumberGenerator.GetInt32(0, 1_000_000);
            return value.ToString("D6");
        }

        private static string HashCode(string code, int userId, string purpose)
        {
            var raw = $"{userId}:{purpose}:{code}";
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(raw));
            return Convert.ToHexString(bytes);
        }
    }
}
