using System.Threading.Tasks;
using Servixa.Shared.DTOs.Chat;
using Servixa.Shared.Commen.Responses;

namespace Servixa.Abstractions.Interfaces
{
    public interface IChatService
    {
        Task<ApiResponse<ChatMessageDto>> SendMessageAsync(int userId, string message);
    }
}
