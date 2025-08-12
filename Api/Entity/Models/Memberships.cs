using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class Memberships 
    {
        public int id { get; set; }
        public int MemberShipTypeId { get; set; }
        public MemberShipType MemberShipType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Active { get; set; }
        public int VehicleId { get; set; }
        public List<Vehicle> Vehicles { get; set; } 

    }
}
