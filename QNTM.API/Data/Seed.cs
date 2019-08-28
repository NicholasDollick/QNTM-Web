using System.Collections.Generic;
using System.Linq;
using BCrypt;
using Newtonsoft.Json;
using QNTM.API.Models;

namespace QNTM.API.Data
{
    public class Seed
    {
        public static void SeedUsers(DataContext context)
        {
            if(!context.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");

                var users = JsonConvert.DeserializeObject<List<User>>(userData);

                foreach(var user in users)
                {
                    string passwordHash, passwordSalt;
                    CreatePasswordHash("password", out passwordHash, out passwordSalt);

                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    user.Username = user.Username.ToLower();
                    context.Users.Add(user);
                }
                context.SaveChanges();
            }
        }

        private static void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
        {
            passwordSalt = BCryptHelper.GenerateSalt(8);
            passwordHash = BCryptHelper.HashPassword(password, passwordSalt);
        }
    }
}