using Microsoft.AspNetCore.SignalR;

namespace Angularintigration.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(
            string user,
            string message)
        {
            await Clients.All.SendAsync(
                "ReceiveMessage",
                user,
                message
            );
        }
    }
}