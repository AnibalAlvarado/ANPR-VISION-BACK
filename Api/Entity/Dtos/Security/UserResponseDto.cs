using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
<<<<<<< HEAD
using Entity.Dtos.Login;
=======
>>>>>>> 6ca5cd43fe20de25915aa779da030276258b25d7

namespace Entity.Dtos.Security
{
    public class UserResponseDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
<<<<<<< HEAD
        public List<string> Roles { get; set; } = new();
        public string Token { get; set; } = null!;

        
        public int PersonId { get; set; }
        public string? FirstName { get; set; }  
        public string? LastName { get; set; }   

        public ClientLiteDto? Client { get; set; }
        //public List<VehicleLiteDto> Vehicles { get; set; } = new();

=======

        public List<string> Roles { get; set; } = new List<string>();

        public string Token { get; set; }

        //public string Role { get; set; } = null!;
>>>>>>> 6ca5cd43fe20de25915aa779da030276258b25d7
    }
}
