using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class PersonParking : BaseModel
    {
        public int ParkingId { get; set; }
        public int PersonId { get; set; }

        public Person Person { get; set; }
        public Parking Parking { get; set; }
    }
}
