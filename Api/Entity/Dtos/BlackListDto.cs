using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos
{
    public class BlackListDto
    {
        public string Reason { get; set; }
        public DateTime RestrictionDate { get; set; }
        public VehicleDto Vehicle { get; set; }
    }
}
