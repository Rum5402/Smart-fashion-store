using Microsoft.AspNetCore.SignalR;

namespace Fashion.Api.Hubs
{
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            await Clients.All.SendAsync("UserConnected", Context.ConnectionId);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
            await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
        }

        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("UserJoinedGroup", Context.ConnectionId, groupName);
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("UserLeftGroup", Context.ConnectionId, groupName);
        }

        public async Task SendFittingRoomRequest(string message)
        {
            await Clients.Group("staff").SendAsync("FittingRoomRequest", Context.ConnectionId, message);
        }

        public async Task SendFittingRoomResponse(string userId, string message)
        {
            await Clients.User(userId).SendAsync("FittingRoomResponse", message);
        }

        public async Task SendPersonalMessage(string targetUserId, string message)
        {
            await Clients.User(targetUserId).SendAsync("PersonalMessage", Context.ConnectionId, message);
        }

        public async Task SendGeneralNotification(string title, string message)
        {
            await Clients.All.SendAsync("GeneralNotification", title, message);
        }

        public async Task SendToStaff(string message)
        {
            await Clients.Group("staff").SendAsync("StaffNotification", Context.ConnectionId, message);
        }

        public async Task SendToCustomers(string message)
        {
            await Clients.Group("customers").SendAsync("CustomerNotification", Context.ConnectionId, message);
        }

        public async Task SendToGuests(string message)
        {
            await Clients.Group("guests").SendAsync("GuestNotification", Context.ConnectionId, message);
        }

        public async Task SendToAll(string message)
        {
            await Clients.All.SendAsync("BroadcastMessage", Context.ConnectionId, message);
        }

        public async Task SendToSpecificGroup(string groupName, string method, params object[] args)
        {
            await Clients.Group(groupName).SendAsync(method, args);
        }

        public async Task SendToSpecificUser(string userId, string method, params object[] args)
        {
            await Clients.User(userId).SendAsync(method, args);
        }
    }
} 