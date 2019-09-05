using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using QNTM.API.Data;

namespace QNTM.API.Hubs
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PrivateMessageHub : Hub
    {
        private IUserHandler _userHandler;   

        public PrivateMessageHub(IUserHandler userHandler)
        {
            _userHandler = userHandler;
            _userHandler.InitDict();
        }

        public async Task Leave()
        {
            _userHandler.RemoveFromDict(Context.User.Identity.Name);
            
            await Clients.AllExcept(new List<string> { Context.ConnectionId }).SendAsync("Disconnected", Context.User.Identity.Name);
        }

        public async Task Join()
        {
            /*
            * if (new user)
            * else (return user)
             */
            Console.WriteLine(Context.User.Identity.Name);
            Console.WriteLine(Context.ConnectionId);
            if (!_userHandler.UpdateDict(Context.User.Identity.Name, Context.ConnectionId))
            {
                var list = _userHandler.GetAllOtherUsers(Context.User.Identity.Name).ToList();
                Console.WriteLine("Start...");
                foreach(var item in list)
                {
                    Console.WriteLine(item.Username);
                }
                Console.WriteLine("Done...");
                await Clients.AllExcept(new List<string> { Context.ConnectionId }).SendAsync("Now Online", Context.User.Identity.Name);
            } else {}

            await Clients.Client(Context.ConnectionId).SendAsync("Joined", _userHandler.GetUserData(Context.User.Identity.Name));
            
            await Clients.Client(Context.ConnectionId).SendAsync("OnlineUsers", _userHandler.GetAllOtherUsers(Context.User.Identity.Name));
        }

        public Task SendDirectMessage(string toUser, string message)
        {
            Console.WriteLine(Context.User.Identity.Name);
            var userFrom = _userHandler.GetUserData(Context.User.Identity.Name);
            var sendToUser = _userHandler.GetUserData(toUser);
            return Clients.Client(sendToUser.ConnectionId).SendAsync("SendPm", message, userFrom);
        }
    }
}