using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class RegisteredVehicles : BaseModel
    {
       
        public DateTime EntryDate { get; set; }
        public DateTime? ExitDate { get; set; }
        public int VehiceId { get; set; }
        public Vehicle Vehicle { get; set; }

        public Slots Slot { get; set; }

    }
}
