namespace QNTM.API.Dtos
{
    public class CreateMessageDto
    {
        public string SenderUsername { get; set; }
        public string RecipientUsername { get; set; }
        public string Content { get; set; }
    }
}