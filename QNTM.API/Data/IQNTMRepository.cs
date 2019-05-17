using System.Collections.Generic;
using System.Threading.Tasks;
using QNTM.API.Helpers;
using QNTM.API.Models;

namespace QNTM.API.Data
{
    public interface IQNTMRepository
    {
        void Add<T>(T entity) where T: class;
        Task<bool> SaveAll();
        Task<User> GetUser(int id);
        Task<IEnumerable<User>> GetUsers();
        Task<Message> GetMessage(int id);
        Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams);
        Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId);
    }
}