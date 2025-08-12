using Entity.Models;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos
{
    public class VehicleDto:GenericDto
    {
        public string Plate { get; set; } 

        public string Color { get; set; }
        public int TypeVehicleId { get; set; }

        public int ClientId { get; set; }

    }
}
