using NotificationService.Application.Models;
using NotificationService.Application.Repositories;
using NotificationService.Application.UseCase.Interfaces;
using NotificationService.Domain.Entities;
using Shared.Common;

namespace NotificationService.Application.UseCase.Implementaions
{
    public class NotificationUseCase : INotificationUseCase
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationUseCase(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<Guid> CreateNotificationAsync(CreateNotificationRequest request)
        {
            var notification = new Notification
            {
                UserId = request.UserId,
                Title = request.Title,
                Message = request.Message,
                Channel = request.Channel,
                IsRead = false,
            };

            await _notificationRepository.AddAsync(notification);
            await _notificationRepository.SaveChangesAsync();

            return notification.Id;
        }

        public async Task<ApiResponse<List<GetNotificationResponse>>> GetNotificationsByUserIdAsync(Guid userId)
        {
            var notifications = await _notificationRepository.GetByUserIdAsync(userId);

            if (notifications == null || !notifications.Any())
            {
                return ApiResponse<List<GetNotificationResponse>>.Fail(MessageId.E0005);
            }

            var response = notifications.Select(n => new GetNotificationResponse
            {
                Id = n.Id,
                UserId = n.UserId,
                Title = n.Title,
                Message = n.Message,
                Channel = n.Channel,
                IsRead = n.IsRead,
                IsSent = n.IsSent,
                SentAt = n.SentAt
            }).ToList();

            return ApiResponse<List<GetNotificationResponse>>.Ok(response);
        }

        public async Task<ApiResponse<List<GetNotificationResponse>>> GetUnreadNotificationsByUserIdAsync(Guid userId)
        {
            var notifications = await _notificationRepository.GetUnreadByUserIdAsync(userId);

            if (notifications == null || !notifications.Any())
            {
                return ApiResponse<List<GetNotificationResponse>>.Fail(MessageId.E0005);
            }

            var response = notifications.Select(n => new GetNotificationResponse
            {
                Id = n.Id,
                UserId = n.UserId,
                Title = n.Title,
                Message = n.Message,
                Channel = n.Channel,
                IsRead = n.IsRead,
                IsSent = n.IsSent,
                SentAt = n.SentAt
            }).ToList();

            return ApiResponse<List<GetNotificationResponse>>.Ok(response);
        }

        public async Task<ApiResponse<GetNotificationResponse>> MarkNotificationAsReadAsync(Guid notificationId)
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId);
            if (notification == null)
            {
                return ApiResponse<GetNotificationResponse>.Fail(MessageId.E0005);
            }

            notification.IsRead = true;
            await _notificationRepository.UpdateAsync(notification);
            await _notificationRepository.SaveChangesAsync();

            var response = new GetNotificationResponse
            {
                Id = notification.Id,
                UserId = notification.UserId,
                Title = notification.Title,
                Message = notification.Message,
                Channel = notification.Channel,
                IsRead = notification.IsRead,
                IsSent = notification.IsSent,
                SentAt = notification.SentAt
            };

            return ApiResponse<GetNotificationResponse>.Ok(response);
        }

        public async Task<ApiResponse<GetNotificationResponse>> DeleteNotificationAsync(Guid notificationId)
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId);
            if (notification == null)
            {
                return ApiResponse<GetNotificationResponse>.Fail(MessageId.E0005);
            }

            await _notificationRepository.SoftDeleteAsync(notification);
            await _notificationRepository.SaveChangesAsync();

            var response = new GetNotificationResponse
            {
                Id = notification.Id,
                UserId = notification.UserId,
                Title = notification.Title,
                Message = notification.Message,
                Channel = notification.Channel,
                IsRead = notification.IsRead,
                IsSent = notification.IsSent,
                SentAt = notification.SentAt
            };

            return ApiResponse<GetNotificationResponse>.Ok(response);
        }
    }
}
