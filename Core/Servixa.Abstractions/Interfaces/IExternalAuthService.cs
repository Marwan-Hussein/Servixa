using Servixa.Shared.DTOs.Auth;

namespace Servixa.Abstractions.Interfaces
{
    public interface IExternalAuthService
    {
        Task<ExternalAuthResponseDto> ProcessExternalLoginAsync(ExternalAuthDto dto);
    }
}
