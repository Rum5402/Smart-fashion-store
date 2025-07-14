using Fashion.Api.Hubs;
using Fashion.Contract.Interface;
using Microsoft.AspNetCore.SignalR;

namespace Fashion.Api.Services
{
    public class NotificationHubService : INotificationHub
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationHubService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendToGroupAsync(string groupName, string method, params object[] args)
        {
            await _hubContext.Clients.Group(groupName).SendAsync(method, args);
        }

        public async Task SendToUserAsync(string userId, string method, params object[] args)
        {
            await _hubContext.Clients.User(userId).SendAsync(method, args);
        }
    }
} 