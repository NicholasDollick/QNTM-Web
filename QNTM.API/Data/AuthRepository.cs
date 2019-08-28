using System.Threading.Tasks;
using QNTM.API.Models;
using BCrypt;
using Microsoft.EntityFrameworkCore;

namespace QNTM.API.Data
{
    public class AuthRepository : IAuthRepositroy
    {
        private readonly DataContext _context;            
        public AuthRepository(DataContext context)
        {
            _context = context;  
        }
        
        public async Task<User> Login(string username, string password)
        {
            var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(x => x.Username == username);

            if(!VerifyPasswordHash(password, user.PasswordHash))
                return null;
            
            return user;
        }

        private bool VerifyPasswordHash(string password, string passwordHash)
        {
            if(!BCryptHelper.CheckPassword(password, passwordHash))
                return false;
            return true;
        }

        public async Task<User> Register(User user, string password, string publicKey, string keyHash)
        {
            string passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.PublicKey = publicKey;
            user.PrivateKeyHash = keyHash;

            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
        {
            passwordSalt = BCryptHelper.GenerateSalt(8);
            passwordHash = BCryptHelper.HashPassword(password, passwordSalt);
        }

        public async Task<bool> UserExists(string username)
        {
            if(await _context.Users.AnyAsync(x => x.Username == username))
                return true;
            return false;
        }
    }
}