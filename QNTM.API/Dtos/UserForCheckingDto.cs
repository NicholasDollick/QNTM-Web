using System.ComponentModel.DataAnnotations;

namespace QNTM.API.Dtos
{
    public class UserForCheckingDto
    {
        [Required]
        public string Username { get; set; }
    }
}