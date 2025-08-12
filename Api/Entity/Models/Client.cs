using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class Client : BaseModel
    {

        public string Name { get; set; }
        public int  PersonaId { get; set; }
        public Person Person { get; set; }
    }
}
