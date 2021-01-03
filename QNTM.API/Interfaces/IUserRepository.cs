using System.Collections.Generic;
using System.Threading.Tasks;
using QNTM.API.Helpers;
using QNTM.API.Models;

namespace QNTM.API.Interfaces
{
    public interface IUserRepository
    {
        void Update(User user);
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByUsernameAsync(string username);
        // Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);
        Task<User> GetUserAsync(string username);
    }
}