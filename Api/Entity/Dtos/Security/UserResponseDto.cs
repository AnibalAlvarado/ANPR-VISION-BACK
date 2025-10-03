using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos.Security
{
    public class UserResponseDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;

        public List<string> Roles { get; set; } = new List<string>();

        public string Token { get; set; }

        //public string Role { get; set; } = null!;
    }
}
