using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using QNTM.API.Data;

namespace QNTM.API.Hubs
{
    [Authorize]
    public class PresenceHub : Hub
    {

        private readonly OnlineUsers _users;

        public PresenceHub(OnlineUsers users)
        {
            _users = users;
        }

        public override async Task OnConnectedAsync()
        {
            await _users.UserConnected(Context.User.Identity.Name, Context.ConnectionId);
            await Clients.Others.SendAsync("UserIsOnline", Context.User.Identity.Name);

            var currentUsers = await _users.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers", currentUsers);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await _users.UserDisconnected(Context.User.Identity.Name, Context.ConnectionId);
            await Clients.Others.SendAsync("UserIsOffline", Context.User.Identity.Name);

            var currentUsers = await _users.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers", currentUsers);

            await base.OnDisconnectedAsync(exception);
        }
    }
}