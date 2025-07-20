using Fashion.Contract.Interface;
using Fashion.Core.Entities;
using Fashion.Core.Enums;
using Fashion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fashion.Service.FittingRoomServices
{
    public class FittingRoomService : IFittingRoomService
    {
        private readonly FashionDbContext _context;
        private readonly INotificationHub _notificationHub;
        private readonly INotificationService _notificationService;

        public FittingRoomService(FashionDbContext context, INotificationHub notificationHub, INotificationService notificationService)
        {
            _context = context;
            _notificationHub = notificationHub;
            _notificationService = notificationService;
        }

        public async Task<FittingRoomRequest> CreateRequestAsync(int userId, int itemId)
        {
            // Check if user exists
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);

            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            // Check if item exists
            var item = await _context.Items
                .FirstOrDefaultAsync(i => i.Id == itemId && !i.IsDeleted);

            if (item == null)
            {
                throw new InvalidOperationException("Item not found");
            }

            // Check if user already has a new request for this item
            var existingRequest = await _context.FittingRoomRequests
                .FirstOrDefaultAsync(f => f.UserId == user.Id && f.ItemId == itemId && 
                                        f.Status == FittingRoomStatus.NewRequest && !f.IsDeleted);

            if (existingRequest != null)
            {
                throw new InvalidOperationException("You already have a pending request for this item");
            }

            var request = new FittingRoomRequest
            {
                UserId = user.Id,
                ItemId = itemId,
                Status = FittingRoomStatus.NewRequest,
                StaffMessage = "The item will be ready in the fitting room within 2 minutes",
                CreatedAt = DateTime.UtcNow
            };

            _context.FittingRoomRequests.Add(request);
            await _context.SaveChangesAsync();

            // Create notification automatically
            await _notificationService.CreateFittingRoomNotificationAsync(userId, itemId, request.Id);

            // Send immediate real-time notification to user
            await _notificationHub.SendToUserAsync(userId.ToString(), "FittingRoomResponse", 
                $"The item will be ready in the fitting room within 2 minutes");

            // Send real-time notification to staff
            await _notificationHub.SendToGroupAsync("staff", "FittingRoomRequest", 
                $"New fitting room request from {user.Name} for {item.Name}");

            // Automatically complete the request after 2 minutes
            _ = Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromMinutes(2));
                await AutoCompleteRequestAsync(request.Id);
            });

            return request;
        }

        private async Task AutoCompleteRequestAsync(int requestId)
        {
            try
            {
                var request = await _context.FittingRoomRequests
                    .Include(f => f.User)
                    .Include(f => f.Item)
                    .FirstOrDefaultAsync(f => f.Id == requestId && !f.IsDeleted);

                if (request != null && request.Status == FittingRoomStatus.NewRequest)
                {
                    request.Status = FittingRoomStatus.Completed;
                    request.StaffMessage = "The item is ready in the fitting room! You can go to any available room";
                    request.HandledAt = DateTime.UtcNow;
                    request.UpdatedAt = DateTime.UtcNow;

                    await _context.SaveChangesAsync();

                    // Send real-time notification to user
                    if (request.User != null)
                    {
                        await _notificationHub.SendToUserAsync(request.UserId.ToString(), "FittingRoomResponse", 
                            $"The item is ready in the fitting room! You can go to any available room");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error but don't throw to avoid breaking the background task
                Console.WriteLine($"Error auto-completing fitting room request {requestId}: {ex.Message}");
            }
        }

        public async Task<FittingRoomRequest?> UpdateRequestStatusAsync(int requestId, FittingRoomStatus status, int? staffId, string? message)
        {
            var request = await _context.FittingRoomRequests
                .FirstOrDefaultAsync(f => f.Id == requestId && !f.IsDeleted);

            if (request == null)
                return null;

            request.Status = status;
            request.StaffMessage = message;
            request.HandledByStaffId = staffId;
            request.HandledAt = DateTime.UtcNow;
            request.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Send real-time notification to user
            if (request.User != null)
            {
                var statusMessage = status switch
                {
                    FittingRoomStatus.NewRequest => $"Your request for '{request.Item?.Name}' has been received!",
                    FittingRoomStatus.Completed => "Thank you for using our fitting room!",
                    FittingRoomStatus.Cancelled => "Your fitting room request has been cancelled.",
                    _ => "Your fitting room request status has been updated."
                };

                await _notificationHub.SendToUserAsync(request.UserId.ToString(), "FittingRoomResponse", statusMessage);
            }

            return request;
        }

        public async Task<FittingRoomRequest?> GetRequestByIdAsync(int requestId)
        {
            return await _context.FittingRoomRequests
                .Include(f => f.User)
                .Include(f => f.Item)
                .FirstOrDefaultAsync(f => f.Id == requestId && !f.IsDeleted);
        }

        public async Task<IEnumerable<FittingRoomRequest>> GetNewRequestsAsync()
        {
            return await _context.FittingRoomRequests
                .Include(f => f.User)
                .Include(f => f.Item)
                .Where(f => f.Status == FittingRoomStatus.NewRequest && !f.IsDeleted)
                .OrderBy(f => f.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<FittingRoomRequest>> GetUserRequestsAsync(int userId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);

            if (user == null)
                return new List<FittingRoomRequest>();

            return await _context.FittingRoomRequests
                .Include(f => f.User)
                .Include(f => f.Item)
                .Where(f => f.UserId == user.Id && !f.IsDeleted)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<FittingRoomRequest>> GetAllRequestsAsync()
        {
            return await _context.FittingRoomRequests
                .Include(f => f.User)
                .Include(f => f.Item)
                .Where(f => !f.IsDeleted)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();
        }
    }

    public interface IFittingRoomService
    {
        Task<FittingRoomRequest> CreateRequestAsync(int userId, int itemId);
        Task<FittingRoomRequest?> UpdateRequestStatusAsync(int requestId, FittingRoomStatus status, int? staffId, string? message);
        Task<FittingRoomRequest?> GetRequestByIdAsync(int requestId);
        Task<IEnumerable<FittingRoomRequest>> GetNewRequestsAsync();
        Task<IEnumerable<FittingRoomRequest>> GetUserRequestsAsync(int userId);
        Task<IEnumerable<FittingRoomRequest>> GetAllRequestsAsync();
    }
} 