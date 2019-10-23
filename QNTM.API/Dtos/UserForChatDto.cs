using System.Collections.Generic;
using QNTM.API.Models;

namespace QNTM.API.Dtos
{
    public class UserForChatDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PublicKey { get; set; }
        public string PhotoUrl { get; set; }
        public ICollection<PhotoForReturnDto> Photos { get; set; }
    }
}