using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QNTM.API.Models;

namespace QNTM.API.Data
{
    // a wrapper class to persist the dictionary
    public class OnlineUsers
    {
        private readonly ConcurrentDictionary<string, ChatUserData> _onlineUsers = new ConcurrentDictionary<string, ChatUserData>();

        public ConcurrentDictionary<string, ChatUserData> Users => _onlineUsers;

        private static readonly Dictionary<string, List<string>> UsersOnline = 
                new Dictionary<string, List<string>>();

        public Task UserConnected(string username, string connectionID)
        {
            lock(UsersOnline)
            {
                if(UsersOnline.ContainsKey(username))
                {
                    UsersOnline[username].Add(connectionID);
                }
                else
                {
                    UsersOnline.Add(username, new List<string>{connectionID});
                }
            }
            return Task.CompletedTask;
        }

        public Task UserDisconnected(string username, string connectionID)
        {
            lock(UsersOnline)
            {
                if(!UsersOnline.ContainsKey(username))
                    return Task.CompletedTask;

                UsersOnline[username].Remove(connectionID);
                if(UsersOnline[username].Count == 0)
                {
                    UsersOnline.Remove(username);
                }
            }

            return Task.CompletedTask;
        }

        public Task<string[]> GetOnlineUsers()
        {
            string[] onlineUsers;
            lock(UsersOnline)
            {
                onlineUsers = UsersOnline.OrderBy(k => k.Key).Select(k => k.Key).ToArray();
            }

            return Task.FromResult(onlineUsers);
        }
    }
}