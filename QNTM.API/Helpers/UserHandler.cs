using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using QNTM.API.Models;

namespace QNTM.API.Helpers
{
    public class UserHandler
    {
        private ConcurrentDictionary<string, ChatUserData> _onlineUsers { get; set; } = new ConcurrentDictionary<string, ChatUserData>();

        public bool UpdateDict (string name, string connectionId)
        {
            var userIsOnline = _onlineUsers.ContainsKey(name);

            var userData = new ChatUserData
            {
                Username = name,
                ConnectionId = connectionId
            };

            _onlineUsers.AddOrUpdate(name, userData, (key, value) => userData);

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