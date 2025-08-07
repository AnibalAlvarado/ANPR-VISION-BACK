using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class RatesType : BaseModel
    {
     
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Rates> Rates { get; set; }
    }
}
