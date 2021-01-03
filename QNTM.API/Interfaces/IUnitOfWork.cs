using System.Threading.Tasks;

namespace QNTM.API.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository {get; }
        IMessageRepository MessageRepository {get;}
        Task<bool> Complete();
        bool HasChanges();
    }
}