using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApp.Core.DTOs
{
    public class UserDto : BaseDto
    {
        // To return user.
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
