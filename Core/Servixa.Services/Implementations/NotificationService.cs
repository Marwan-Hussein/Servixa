using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Servixa.Abstractions.Interfaces;
using Servixa.Shared.DTOs.Notification;
using Servixa.Domain.Contracts.UnitOfWorkPattern;
using Servixa.Shared.Commen.Responses;
using Servixa.Domain.Models.NotificationEntity;
using Servixa.Domain.Specifications;

namespace Servixa.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<IEnumerable<NotificationResponseDto>>> GetUserNotificationsAsync(int userId)
        {
            var repo = _unitOfWork.GetReposatory<Notification, int>();
            var spec = new NotificationByUserSpecification(userId);
            var notifications = await repo.GetAllWithSpecAsync(spec);

            var responseDtos = notifications.Select(MapToResponseDto).ToList();
            return new ApiResponse<IEnumerable<NotificationResponseDto>>(responseDtos, "Ok");
        }

        public async Task<ApiResponse<bool>> MarkAsReadAsync(int notificationId)
        {
            var repo = _unitOfWork.GetReposatory<Notification, int>();
            var notification = await repo.GetByIdAsync(notificationId);

            if (notification == null)
            {
                return new ApiResponse<bool>(false, "Not Found", 404);
            }

            notification.IsRead = true;
            repo.UpdateAsync(notification);
            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse<bool>(true, "Marked as read");
        }

        public async Task<ApiResponse<bool>> MarkAllAsReadAsync(int userId)
        {
            var repo = _unitOfWork.GetReposatory<Notification, int>();
            var spec = new NotificationByUserSpecification(userId, true);
            var unreadNotifications = await repo.GetAllWithSpecAsync(spec);

            foreach (var notification in unreadNotifications)
            {
                notification.IsRead = true;
                repo.UpdateAsync(notification);
            }

            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse<bool>(true, "All marked as read");
        }

        public async Task<ApiResponse<NotificationResponseDto>> CreateNotificationAsync(int receiverId, string title, string message)
        {
            var repo = _unitOfWork.GetReposatory<Notification, int>();
            var notification = new Notification
            {
                ReceiverId = receiverId,
                Title = title,
                Body = message,
                IsRead = false
            };

            await repo.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse<NotificationResponseDto>(MapToResponseDto(notification), "Created");
        }

        private NotificationResponseDto MapToResponseDto(Notification notification)
        {
            return new NotificationResponseDto
            {
                Id = notification.Id,
                ReceiverId = notification.ReceiverId,
                Title = notification.Title,
                Message = notification.Body,
                IsRead = notification.IsRead,
                CreatedAt = notification.CreatedAt
            };
        }
    }
}
