using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace Angularintigration.Hubs
{
    public class ChatHub : Hub
    {
        private static ConcurrentDictionary<int, string> OnlineUsers =new ConcurrentDictionary<int, string>();

        public static List<int> GetOnlineUsers()
        {
            return OnlineUsers.Keys.ToList();
        }
   

        public async Task UserOnline(int userId)
        {
            OnlineUsers.AddOrUpdate(userId,Context.ConnectionId,(key, oldValue) =>Context.ConnectionId);

            await Clients.All.SendAsync("UserStatus",userId,true);
        }

        public async Task SendMessage(int senderId,int receiverId,string message)
        {
            var now =DateTime.Now;

      
            if (OnlineUsers.TryGetValue(receiverId,out var receiverConn))
            { 
                await Clients.Client(receiverConn).SendAsync("ReceiveMessage",senderId,receiverId,message,now);
            }

            if (OnlineUsers.TryGetValue(senderId,out var senderConn))
            {
                await Clients.Client(senderConn).SendAsync("ReceiveMessage",senderId,receiverId,message,now);
            }
        }

        public override async Task OnDisconnectedAsync(Exception? ex)
        {
            var user =OnlineUsers.FirstOrDefault(x => x.Value ==Context.ConnectionId);

            if (user.Key != 0)
            {
                await Task.Delay(3000);

                if (OnlineUsers.TryGetValue(user.Key,out var connId) &&connId == Context.ConnectionId)
                {
                    OnlineUsers.TryRemove(user.Key,out _);

                    await Clients.All.SendAsync("UserStatus",user.Key,false);
                }
            }

            await base.OnDisconnectedAsync(ex);
        }
    }
}