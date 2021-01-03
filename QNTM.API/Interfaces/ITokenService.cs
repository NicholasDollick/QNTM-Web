using System.Threading.Tasks;
using QNTM.API.Models;

namespace QNTM.API.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(User user);
    }
}