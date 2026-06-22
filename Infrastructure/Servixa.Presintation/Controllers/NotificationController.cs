using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Servixa.Abstractions.Interfaces;
using Servixa.Shared.DTOs.Notification;

namespace Servixa.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserNotifications(int userId)
        {
            var result = await _notificationService.GetUserNotificationsAsync(userId);
            return Ok(result);
        }

        [HttpPut("{notificationId}/read")]
        public async Task<IActionResult> MarkAsRead(int notificationId)
        {
            var result = await _notificationService.MarkAsReadAsync(notificationId);
            return Ok(result);
        }

        [HttpPut("user/{userId}/read-all")]
        public async Task<IActionResult> MarkAllAsRead(int userId)
        {
            var result = await _notificationService.MarkAllAsReadAsync(userId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotification(int receiverId, string title, string message)
        {
            var result = await _notificationService.CreateNotificationAsync(receiverId, title, message);
            return Ok(result);
        }
    }
}
