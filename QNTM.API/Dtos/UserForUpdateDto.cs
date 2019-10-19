using System.Collections.Generic;
using QNTM.API.Models;

namespace QNTM.API.Dtos
{
    public class UserForUpdateDto
    {
        public ICollection<User> ActiveChats { get; set; }
    }
}