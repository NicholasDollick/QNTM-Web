namespace QNTM.API.Models
{
    public class ChatUserData
    {
        public string ConnectionId { get; set; }
        public string Username { get; set; }
        public User User { get; set; }
    }
}