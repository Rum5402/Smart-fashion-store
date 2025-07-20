using Fashion.Core.Entities;
using Fashion.Contract.DTOs.Notifications;

namespace Fashion.Contract.Interface
{
    public interface INotificationService
    {
        Task<Notification> CreateFittingRoomNotificationAsync(int userId, int itemId, int fittingRoomRequestId);
        Task<List<NotificationDto>> GetUserNotificationsAsync(string userId);
        Task<bool> RespondToNotificationAsync(int id, string response);
    }
} 