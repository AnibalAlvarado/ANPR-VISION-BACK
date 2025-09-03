using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos
{
    public class RatesTypeDto:GenericDto
    {
        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(100, ErrorMessage = "La descripción no puede superar los 100 caracteres.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "La descripción solo puede contener letras y espacios.")]
        public string? Description { get; set; }
    }
}
