using System.Collections.Concurrent;
using QNTM.API.Models;

namespace QNTM.API.Data
{
    // a wrapper class to persist the dictionary
    public class OnlineUsers
    {
        private readonly ConcurrentDictionary<string, ChatUserData> _onlineUsers = new ConcurrentDictionary<string, ChatUserData>();

        public ConcurrentDictionary<string, ChatUserData> Users => _onlineUsers;
    }
}