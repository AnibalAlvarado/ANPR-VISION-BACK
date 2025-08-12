using Entity.Models;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos
{
    public class VehicleDto
    {
        public string Plate { get; set; }
        public string Color { get; set; }


        public TypeVehicleDto TypeVehicle { get; set; }

    
        public ClientDto Client { get; set; }


        public BlackListDto BlackList { get; set; }
        public MembershipsDto Memberships { get; set; }
        public RegisteredVehiclesDto RegisteredVehicles { get; set; }

    }
}
