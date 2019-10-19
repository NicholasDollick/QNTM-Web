namespace QNTM.API.Models
{
    public class ActiveChat
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PhotoUrl { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}