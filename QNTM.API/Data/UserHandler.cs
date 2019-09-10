using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using QNTM.API.Models;

namespace QNTM.API.Data
{
    public class UserHandler : IUserHandler
    {
        // private ConcurrentDictionary<string, ChatUserData> _onlineUsers { get; set; }
        private readonly OnlineUsers _onlineUsers;

        public UserHandler(OnlineUsers onlineUsers) => _onlineUsers = onlineUsers;

        public bool UpdateDict (string name, string connectionId)
        {
            var userIsOnline = _onlineUsers.GetOnlineUsers.ContainsKey(name);
            Console.WriteLine(userIsOnline);

            var userData = new ChatUserData
            {
                Username = name,
                ConnectionId = connectionId
            };
            Console.WriteLine("Users In Dict");
            _onlineUsers.GetOnlineUsers.AddOrUpdate(name, userData, (key, value) => userData);
            foreach (var item in _onlineUsers.GetOnlineUsers)
            {
                Console.WriteLine(item.Key);
                Console.WriteLine(item.Value);
            }
            Console.WriteLine("Returning");

            return userIsOnline;
        }

        public int GetDictSize()
        {
            return _onlineUsers.GetOnlineUsers.Count;
        }

        public void RemoveFromDict(string name)
        {
            ChatUserData userData;
            _onlineUsers.GetOnlineUsers.TryRemove(name, out userData);
        }

        // returns all users except for the user that made the request
        public IEnumerable<ChatUserData> GetAllOtherUsers(string name)
        {
            return _onlineUsers.GetOnlineUsers.Values.Where(user => user.Username != name);
        }

        public ChatUserData GetUserData(string name)
        {
            ChatUserData user;
            _onlineUsers.GetOnlineUsers.TryGetValue(name, out user);
            return user;
        }
    }
}