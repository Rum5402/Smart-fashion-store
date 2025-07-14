using Fashion.Contract.Interface;
using Fashion.Core.Entities;
using Fashion.Core.Interface;
using System.Text.Json;

namespace Fashion.Service.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationHub _notificationHub;

        public NotificationService(IUnitOfWork unitOfWork, INotificationHub notificationHub)
        {
            _unitOfWork = unitOfWork;
            _notificationHub = notificationHub;
        }

        public async Task<Notification> CreateFittingRoomNotificationAsync(int userId, int itemId, int fittingRoomRequestId)
        {
            var userRepository = _unitOfWork.Repository<User>();
            var itemRepository = _unitOfWork.Repository<Item>();
            var notificationRepository = _unitOfWork.Repository<Notification>();
            
            var user = await userRepository.GetByIdAsync(userId);
            var item = await itemRepository.GetByIdAsync(itemId);

            if (user == null || item == null)
                throw new InvalidOperationException("User or item not found");

            var notification = new Notification
            {
                Type = "FittingRoomRequest",
                Title = "New Fitting Room Request",
                Message = $"New fitting room request from user {user.Name} for product {item.Name}",
                UserId = user.Id,
                ItemId = itemId,
                FittingRoomRequestId = fittingRoomRequestId,
                IsRead = false
            };

            await notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangeAsync();

            // Send real-time notification to admin
            await _notificationHub.SendToGroupAsync("admin", "NewFittingRoomRequest", 
                notification.Id, notification.Message, user.Name, item.Name);

            return notification;
        }

        public async Task<List<NotificationDto>> GetUserNotificationsAsync(string userId)
        {
            var notificationRepository = _unitOfWork.Repository<Notification>();
            var itemRepository = _unitOfWork.Repository<Item>();
            var fittingRoomRepository = _unitOfWork.Repository<FittingRoomRequest>();
            
            var allNotifications = await notificationRepository.GetAllAsync();
            var allItems = await itemRepository.GetAllAsync();
            var allFittingRoomRequests = await fittingRoomRepository.GetAllAsync();
            
            var userNotifications = allNotifications
                .Where(n => n.UserId.HasValue && n.UserId.Value.ToString() == userId && !n.IsDeleted)
                .OrderByDescending(n => n.CreatedAt)
                .ToList();

            return userNotifications.Select(n => MapToDto(n, allItems, allFittingRoomRequests)).ToList();
        }

        public async Task<bool> RespondToNotificationAsync(int id, string response)
        {
            var notificationRepository = _unitOfWork.Repository<Notification>();
            
            var allNotifications = await notificationRepository.GetAllAsync();
            var notification = allNotifications.FirstOrDefault(n => n.Id == id);

            if (notification == null)
                return false;

            notification.AdminResponse = response;
            notification.RespondedAt = DateTime.UtcNow;
            notification.UpdatedAt = DateTime.UtcNow;

            await notificationRepository.UpdateAsync(notification);
            await _unitOfWork.SaveChangeAsync();

            // Send response to user
            if (notification.UserId.HasValue)
            {
                await _notificationHub.SendToUserAsync(notification.UserId.Value.ToString(), 
                    "AdminResponse", response);
            }

            return true;
        }



        private static NotificationDto MapToDto(Notification notification, IEnumerable<Item> items, IEnumerable<FittingRoomRequest> fittingRoomRequests)
        {
            var item = notification.ItemId.HasValue 
                ? items.FirstOrDefault(i => i.Id == notification.ItemId.Value) 
                : null;
                
            var fittingRoomRequest = notification.FittingRoomRequestId.HasValue 
                ? fittingRoomRequests.FirstOrDefault(f => f.Id == notification.FittingRoomRequestId.Value) 
                : null;

            return new NotificationDto
            {
                Id = notification.Id,
                Type = notification.Type,
                Title = notification.Title,
                Message = notification.Message,
                AdminResponse = notification.AdminResponse,
                IsRead = notification.IsRead,
                ItemId = notification.ItemId,
                ItemName = item?.Name,
                FittingRoomRequestId = notification.FittingRoomRequestId,
                FittingRoomStatus = fittingRoomRequest?.Status.ToString(),
                CreatedAt = notification.CreatedAt,
                RespondedAt = notification.RespondedAt,
                ReadAt = notification.ReadAt
            };
        }
    }


} 