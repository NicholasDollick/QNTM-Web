using System.ComponentModel.DataAnnotations;

namespace QNTM.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(64, MinimumLength = 6, ErrorMessage = "You Must Use A Password Length Between 6 and 64")]
        public string Password { get; set; }
    }
}