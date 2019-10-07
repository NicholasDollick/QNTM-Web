using System.Collections.Generic;

namespace QNTM.API.Dtos
{
    public class UserForDetailDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PhotoUrl { get; set; }
        // This field is not really needed, as the only relevant PhotoUrl is the main photo
        // public ICollection<PhotoForReturnDto> Photos { get; set; }
    }
}