using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApp.Core.DTOs
{
    public class RegistrationDto : BaseDto
    {
        // DTO for registering the user. A compilation of User info and User Creds.
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
