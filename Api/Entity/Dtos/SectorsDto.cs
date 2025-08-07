using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos
{
    public class SectorsDto:GenericDto
    {
 
        public int Capacity { get; set; }
        public int ZoneId { get; set; }
        public int TypeVehicleId { get; set; }
    }
}
