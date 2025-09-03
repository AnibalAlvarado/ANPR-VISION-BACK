
using Entity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos
{
    public class ClientDto : GenericDto
    {
        [Required(ErrorMessage = "El ID de la persona es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID de la persona debe ser mayor a 0")]
        public int PersonaId { get; set; }

        public string? Person { get; set; }
    }
}
