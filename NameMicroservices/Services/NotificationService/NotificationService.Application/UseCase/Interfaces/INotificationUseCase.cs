using NotificationService.Application.Models;
using Shared.Common;

namespace NotificationService.Application.UseCase.Interfaces
{
    public interface INotificationUseCase
    {
        Task<Guid> CreateNotificationAsync(CreateNotificationRequest request);
        Task<ApiResponse<List<GetNotificationResponse>>> GetNotificationsByUserIdAsync(Guid userId);
        Task<ApiResponse<List<GetNotificationResponse>>> GetUnreadNotificationsByUserIdAsync(Guid userId);
        Task<ApiResponse<GetNotificationResponse>> MarkNotificationAsReadAsync(Guid notificationId);
        Task<ApiResponse<GetNotificationResponse>> DeleteNotificationAsync(Guid notificationId);
    }
}
