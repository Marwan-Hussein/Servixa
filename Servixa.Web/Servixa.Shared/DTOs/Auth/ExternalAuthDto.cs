namespace Application.Core.DTOs.Auth
{
    public class ExternalAuthDto
    {
        public string Provider { get; set; }
        public string ProviderKey { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string RequestedRole { get; set; }
    }
}