using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
     public class Slots : BaseModel
    {
   

        public string Name { get; set; }
        public bool IsAvailable { get; set; }
        public int SectorsId { get; set; }
        public Sectors Sector { get; set; }

    }
}
