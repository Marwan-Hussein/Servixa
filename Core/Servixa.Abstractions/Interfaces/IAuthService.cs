using Servixa.Shared.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servixa.Abstractions.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterClientAsync(RegisterClientDto dto);
        Task<AuthResponseDto> RegisterWorkerAsync(RegisterWorkerDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        Task ChangePasswordAsync(int userId, ChangePasswordDto dto);
    }
}
