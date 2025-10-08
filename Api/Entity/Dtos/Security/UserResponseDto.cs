using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Dtos.Login;

namespace Entity.Dtos.Security
{
    public class UserResponseDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public List<string> Roles { get; set; } = new();
        public string Token { get; set; } = null!;

        
        public int PersonId { get; set; }
        public string? FirstName { get; set; }  
        public string? LastName { get; set; }   

        public ClientLiteDto? Client { get; set; }
        //public List<VehicleLiteDto> Vehicles { get; set; } = new();

    }
}
