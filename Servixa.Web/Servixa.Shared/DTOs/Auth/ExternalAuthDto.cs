namespace Servixa.Shared.DTOs.Auth
{
    public class ExternalAuthDto
    {
        public string Provider { get; set; } = string.Empty;
        public string ProviderKey { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? RequestedRole { get; set; }
    }
}
