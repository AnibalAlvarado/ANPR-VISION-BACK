using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class Camara : BaseModel
    {
       
        public string Resolution { get; set; }
        public string Ip { get; set; }
        public int ParkingId { get; set; }
        public Parking Parking { get; set; }
    }
}
