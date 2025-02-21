using Microsoft.AspNetCore.SignalR;

public class NotificationHub : Hub
{
    public async Task SendNotification(string userId, string message)
    {
        await Clients.All.SendAsync("ReceiveNotification", message);
    }
}
