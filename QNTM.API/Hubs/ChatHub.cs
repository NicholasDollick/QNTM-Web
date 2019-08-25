using System;
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

        public void ChatMessages(string jsonData)
        {
            Clients.All.SendAsync("chatMessages", jsonData);
        }

        // sends to single user
        // based on CliamTypes.NameIdentifier
        public Task SendPrivateMessage(string user, string message)
        {
            Console.WriteLine(user);
            Console.WriteLine(Context.User.Identity);
            Console.WriteLine(Context.ConnectionId);
            // Console.WriteLine(ConnectedUse);
            return Clients.User(user).SendAsync("RecievedMessage", message);
        }

        // Code for "groups"
        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has joined the group {groupName}");
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}");
        }

        public Task SendMessageToGroup(string group, string message)
        {
            return Clients.Group(group).SendAsync("RecievedMessage", message);
        }
    }
}