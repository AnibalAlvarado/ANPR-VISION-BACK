using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos
{
    public class TypeVehicleDto
    {
        public string Name { get; set; }
        public Vehicle Vehicle { get; set; }

        public List<Sectors> sectors { get; set; }
        public List<Rates> Rates { get; set; }
    }
}
