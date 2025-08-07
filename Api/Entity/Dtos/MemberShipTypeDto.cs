using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos
{
    public class MemberShipTypeDto:GenericDto
    {
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; }
    }
}
