using Entity.DTOs;
using Entity.Model;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos
{
    public class ClientDto
    {
        public string Name { get; set; }
        public int PersonaId { get; set; }
        public Person Person { get; set; } // opcional, si quieres mostrar datos de la persona
    }
}
