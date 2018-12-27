using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;

namespace WebApplication3.Domain.Security
{
    public class User : IdentityUser
    {

        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
