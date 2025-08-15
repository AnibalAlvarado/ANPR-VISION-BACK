using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class Zones : GenericModel
    {
        public int ParkingId { get; set; }

        public Parking Parking { get; set; } = null!;
        public ICollection<Sectors> Sectors { get; set; } = new List<Sectors>();

    }
}
