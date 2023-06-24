using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApp.Core
{
    public static class Constants
    {
        public const string secretKey = "This is a super secret key";
        public static HashSet<string> InvalidatedTokens { get; set; }
    }
}
