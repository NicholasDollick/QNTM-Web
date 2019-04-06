using System.Collections.Generic;
using System.Threading.Tasks;
using QNTM.API.Helpers;
using QNTM.API.Models;

namespace QNTM.API.Data
{
    public interface IQNTMRepository
    {
        Task<Message> GetMessage(int id);
        Task<PagedList<Message>> GetMessagesForUser();
        Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId);
    }
}