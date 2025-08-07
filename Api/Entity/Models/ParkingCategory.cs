using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
     public class ParkingCategory : BaseModel
    {
       
        public string code { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }

        public List<Parking> Parking { get; set; }
    }
}
