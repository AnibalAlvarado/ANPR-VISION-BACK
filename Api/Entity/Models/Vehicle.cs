using Entity.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class Vehicle : BaseModel
    {
    
        public string Plate { get; set; }
        public string color { get; set; }

      

        public int TypeVehicleId { get; set; }
        public TypeVehicle? TypeVehicle { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }

    
        public Memberships Memberships { get; set; }



    }
}
