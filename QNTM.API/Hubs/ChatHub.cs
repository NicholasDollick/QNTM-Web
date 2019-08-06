using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using QNTM.API.Data;

namespace QNTM.API.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("RecievedMessage", user, message);
        }


        public void SendToAll(string name, string message)
        {
            Clients.All.SendAsync("sendToAll", name, message);
        }
    }
}