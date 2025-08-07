using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos
{
    public class MembershipsDto:GenericDto
    {
        public int MemberShipTypeId { get; set; }
        public MemberShipType MemberShipType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Active { get; set; }
        public int VehicleId { get; set; }
    }
}
