namespace Application.Core.DTOs.Auth
{
    public class ExternalAuthResponseDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public UserDto User { get; set; }
        public bool NeedsRoleSelection { get; set; }
        public string Provider { get; set; }
        public string ProviderKey { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
