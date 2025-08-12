using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class BlackList
    {
        public int Id { get; set; }

        public string Reason { get; set; }
        public DateTime  RestrictionDate { get; set; }

        public Vehicle Vehicle { get; set; }
    }
}
