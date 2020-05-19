using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using QNTM.API.Data;
using QNTM.API.Models;

namespace QNTM.API.Hubs
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PrivateMessageHub : Hub
    {
        private IUserHandler _userHandler;
        private readonly IQNTMRepository _repo;
        private readonly IMapper _mapper;

        public PrivateMessageHub(IUserHandler userHandler, IQNTMRepository repo, IMapper mapper)
        {
            _userHandler = userHandler;
            _repo = repo;
            _mapper = mapper;
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
                Console.WriteLine(_userHandler.GetDictSize());
                Console.WriteLine("Done...");
                await Clients.AllExcept(new List<string> { Context.ConnectionId }).SendAsync("Now Online", Context.User.Identity.Name);
            } else {}

            await Clients.Client(Context.ConnectionId).SendAsync("Joined", _userHandler.GetUserData(Context.User.Identity.Name));
            
            await Clients.Client(Context.ConnectionId).SendAsync("OnlineUsers", _userHandler.GetAllOtherUsers(Context.User.Identity.Name));
        }

        // routes the message to the correct location
        // adds the message to the db for both users
        public Task SendDirectMessage(string toUser, string message)
        {
            Console.WriteLine(Context.User.Identity.Name);
            var userFrom = _userHandler.GetUserData(Context.User.Identity.Name);
            var sendToUser = _userHandler.GetUserData(toUser);
            

            //TODO: the function fails while attempting to create this message object
            var messageToCreate = new Message { 
                SenderId = _repo.GetUser(Context.User.Identity.Name).Id,
                RecipientId = _repo.GetUser(toUser).Id, Content = message, MessageSent = DateTime.Now 
                };

            Console.WriteLine(messageToCreate);

            var messageToSave = _mapper.Map<Message>(messageToCreate);
            Console.WriteLine(messageToSave);

            _repo.Add(messageToSave);

            if(!(_repo.SaveAll().Result))
                throw new Exception("Message Failed To Save");

            return Clients.Client(sendToUser.ConnectionId).SendAsync("SendPm", message, userFrom);
        }
    }
}