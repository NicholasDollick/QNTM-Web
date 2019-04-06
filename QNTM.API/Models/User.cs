using System.Collections.Generic;

namespace QNTM.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public ICollection<Message> MessagesSent { get; set; }
        public ICollection<Message> MessagesRecieved { get; set; }
    }
}