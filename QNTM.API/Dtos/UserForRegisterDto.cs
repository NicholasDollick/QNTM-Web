using System.ComponentModel.DataAnnotations;

namespace QNTM.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(64, MinimumLength = 6, ErrorMessage = "You Must Use A Password At Least 6 Characters Long")]
        public string Password { get; set; }
        [Required]
        public string PublicKey { get; set; }
        [Required]
        public string PrivateKeyHash { get; set; }
    }
}