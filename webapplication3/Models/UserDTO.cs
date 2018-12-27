using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebApplication3.Models
{
    public class UserDTO
    {
        public string Id { get; set; }
        [Required]
        public string Mail { get; set; }
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Role { get; set; }

        public bool Active { get; set; }
    }
}
