using System.Collections.Generic;
using System.Threading.Tasks;
using Servixa.Shared.DTOs.Notification;
using Servixa.Shared.Commen.Responses;

namespace Servixa.Abstractions.Interfaces
{
    public interface INotificationService
    {
        Task<ApiResponse<IEnumerable<NotificationResponseDto>>> GetUserNotificationsAsync(int userId);
        Task<ApiResponse<bool>> MarkAsReadAsync(int notificationId);
        Task<ApiResponse<bool>> MarkAllAsReadAsync(int userId);
        Task<ApiResponse<NotificationResponseDto>> CreateNotificationAsync(int receiverId, string title, string message);
    }
}
