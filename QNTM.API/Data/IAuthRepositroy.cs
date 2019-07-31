using System.Threading.Tasks;
using QNTM.API.Models;

namespace QNTM.API.Data
{
    public interface IAuthRepositroy
    {
        Task<User> Register(User user, string password, string publicKey, string keyHash);
        Task<User> Login(string username, string password);
        Task<bool> UserExists(string username);
    }
}