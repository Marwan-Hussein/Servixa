namespace Application.Core.Interfaces.Auth
{
    public interface IExternalAuthService
    {
        public Task<ExternalAuthResponseDto> ProcessExternalLoginAsync(ExternalAuthDto dto);
    }
}