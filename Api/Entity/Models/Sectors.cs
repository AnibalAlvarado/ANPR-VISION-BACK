using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class Sectors : BaseModel
    {
       
        public string Name { get; set; }
        public int Capacity { get; set; }
        public int ZoneId { get; set; }
        public Zones Zone { get; set; }
        public List<Slots> Slots { get; set; } 
        public int TypeVehicleId { get; set; }
        public TypeVehicle TypeVehicle { get; set; }

    }
}
