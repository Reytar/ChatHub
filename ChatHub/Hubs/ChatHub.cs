using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ChatServer.Hubs
{
    public class ChatHub : Hub
    {
        public async override Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("OnConnection", Context.ConnectionId);
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
