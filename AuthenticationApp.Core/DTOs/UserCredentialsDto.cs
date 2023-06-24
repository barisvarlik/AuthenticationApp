using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApp.Core.DTOs
{
    public class UserCredentialsDto : BaseDto
    {
        //DTO for Login 
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
