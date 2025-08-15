using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class RatesType : GenericModel
    {
        public string Description { get; set; } = string.Empty;
        public ICollection<Rates> Rates { get; set; } = new List<Rates>();
    }
}
