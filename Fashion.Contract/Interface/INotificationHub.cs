namespace Fashion.Contract.Interface
{
    public interface INotificationHub
    {
        Task SendToGroupAsync(string groupName, string method, params object[] args);
        Task SendToUserAsync(string userId, string method, params object[] args);
    }
} 