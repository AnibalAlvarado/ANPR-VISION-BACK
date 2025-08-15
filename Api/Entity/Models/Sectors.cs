using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class Sectors : GenericModel
    {
        public int Capacity { get; set; }
        public int ZonesId { get; set; }
        public int TypeVehicleId { get; set; }

        public Zones Zones { get; set; } = null!;
        public TypeVehicle TypeVehicle { get; set; } = null!;
        public ICollection<Slots> Slots { get; set; } = new List<Slots>();

    }
}
