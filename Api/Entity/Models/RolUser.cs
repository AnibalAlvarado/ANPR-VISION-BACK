using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class RolUser : BaseModel
    {
        public int RolId { get; set; }
        public int UserId { get; set; }

        public Rol? Rol { get; set; }
        public User? User { get; set; }
    }
}
