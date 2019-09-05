using System.Collections.Generic;
using QNTM.API.Models;

namespace QNTM.API.Data
{
    public interface IUserHandler
    {
        bool UpdateDict(string name, string connectionId);
        void RemoveFromDict(string name);
        IEnumerable<ChatUserData> GetAllOtherUsers(string name);
        ChatUserData GetUserData(string name);
        void InitDict();
    }
}