using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using QNTM.API.Models;

namespace QNTM.API.Data
{
    public class UserHandler : IUserHandler
    {
        private ConcurrentDictionary<string, ChatUserData> _onlineUsers { get; set; }

        public void InitDict()
        {
           _onlineUsers = new ConcurrentDictionary<string, ChatUserData>();
        }
        public bool UpdateDict (string name, string connectionId)
        {
            var userIsOnline = _onlineUsers.ContainsKey(name);
            Console.WriteLine(userIsOnline);

            var userData = new ChatUserData
            {
                Username = name,
                ConnectionId = connectionId
            };

            _onlineUsers.AddOrUpdate(name, userData, (key, value) => userData);
            foreach (var item in _onlineUsers)
            {
                Console.WriteLine(item.Key);
                Console.WriteLine(item.Value);
            }

            return userIsOnline;
        }

        public void RemoveFromDict(string name)
        {
            ChatUserData userData;
            _onlineUsers.TryRemove(name, out userData);
        }

        // returns all users except for the user that made the request
        public IEnumerable<ChatUserData> GetAllOtherUsers(string name)
        {
            return _onlineUsers.Values.Where(user => user.Username != name);
        }

        public ChatUserData GetUserData(string name)
        {
            ChatUserData user;
            _onlineUsers.TryGetValue(name, out user);
            return user;
        }
    }
}