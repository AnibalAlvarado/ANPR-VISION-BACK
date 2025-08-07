using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class Parking : BaseModel
    {
    
        public string Name { get; set; }
        public string Location { get; set; }
        public int ParkingCategoryId { get; set; }
        public ParkingCategory ParkingCategory { get; set; }
        public List<Zones> Zones { get; set; }
        public List<Camara> Camaras { get; set; }
    }
}
