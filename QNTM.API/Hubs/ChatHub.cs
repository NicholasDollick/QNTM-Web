using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using QNTM.API.Data;
using QNTM.API.Dtos;
using QNTM.API.Models;

namespace QNTM.API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IQNTMRepository _repo;
        private readonly IMapper _mapper;
        public ChatHub(IQNTMRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext.Request.Query["user"].ToString();
            var groupName = GetGroupName(Context.User.Identity.Name, otherUser);

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            var messages = await _repo.GetMessageThread(Context.User.Identity.Name, otherUser);
            Console.WriteLine("Messages would be fetched");
            await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(MessageForCreationDto messageForCreationDto)
        {
            Console.WriteLine("Inside the send message function");
            var username = Context.User.Identity.Name;

            if(username == messageForCreationDto.RecipientUsername.ToLower())
                throw new HubException("You can't send a message to yourself");
            
            Console.WriteLine($"Username received in hub: {messageForCreationDto.SenderUsername}");
            var sender = await _repo.GetUser(username);
            Console.WriteLine("fetched sender");
            var recipient = await _repo.GetUser(messageForCreationDto.RecipientUsername);
            Console.WriteLine("fetched recip");

            if (recipient == null)
                throw new HubException("User not found");
            
            var message= new Message
            {
              Sender = sender,
              Recipient = recipient,
              SenderUsername = sender.Username,
              RecipientUsername = recipient.Username,
              Content = messageForCreationDto.Content
            };
            Console.WriteLine($"Created Message: {message}");
            _repo.AddMessage(message);
            Console.WriteLine("message added to repo");

            if(await _repo.SaveAll()) 
            {
                var group = GetGroupName(sender.Username, recipient.Username);
                await Clients.Group(group).SendAsync("NewMessage", _mapper.Map<MessageToReturnDto>(message));
            }
            else
                Console.WriteLine("Something fucked up while trying to save");
        }

        private string GetGroupName(string caller, string other)
        {
            var stringComapre = string.CompareOrdinal(caller, other) < 0;

            return stringComapre ? $"{caller}-{other}" : $"{other}-{caller}";
            
        }










        // public void SendToAll(string name, string message)
        // {
        //     Clients.All.SendAsync("sendToAll", name, message);
        // }

        // public void ChatMessages(string jsonData)
        // {
        //     Clients.All.SendAsync("chatMessages", jsonData);
        // }

        // // sends to single user
        // // based on CliamTypes.NameIdentifier
        // public Task SendPrivateMessage(string user, string message)
        // {
        //     Console.WriteLine(user);
        //     Console.WriteLine(Context.ConnectionId);
        //     Console.WriteLine(Clients.User(user));
        //     Console.WriteLine(Clients.User(userId: user));
        //     Console.WriteLine(Clients.User(userId: Context.ConnectionId));
        //     return Clients.User("1").SendAsync("PrivateMessage", message);
        // }

        // public void SendPm (string toUser, string message)
        // {
        //     string fromUser = Context.ConnectionId;

        // }

        // // Code for "groups"
        // public async Task AddToGroup(string groupName)
        // {
        //     await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        //     await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has joined the group {groupName}");
        // }

        // public async Task RemoveFromGroup(string groupName)
        // {
        //     await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

        //     await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}");
        // }

        // public Task SendMessageToGroup(string group, string message)
        // {
        //     return Clients.Group(group).SendAsync("RecievedMessage", message);
        // }
    }
}