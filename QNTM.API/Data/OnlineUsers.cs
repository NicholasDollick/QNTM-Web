using System.Collections.Concurrent;
using QNTM.API.Models;

namespace QNTM.API.Data
{
    public class OnlineUsers
    {
        private readonly ConcurrentDictionary<string, ChatUserData> _onlineUsers = new ConcurrentDictionary<string, ChatUserData>();

        public ConcurrentDictionary<string, ChatUserData> GetOnlineUsers => _onlineUsers;
    }
}