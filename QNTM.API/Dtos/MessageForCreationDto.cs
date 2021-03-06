using System;

namespace QNTM.API.Dtos
{
    public class MessageForCreationDto
    {
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public DateTime MessageSentDate { get; set; }
        public string Content { get; set; }
        public MessageForCreationDto()
        {
            MessageSentDate = DateTime.Now;
        }
    }
}