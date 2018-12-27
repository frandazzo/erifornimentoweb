using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3.Models
{
    public class LoginResult
    {
        public string Token { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Error { get; set; }

    }
}
