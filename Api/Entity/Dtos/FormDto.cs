using Entity.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos
{

    public class FormDto : GenericDto
    {
        [Required(ErrorMessage = "La descripción del formulario es obligatoria")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "La descripción debe tener entre 5 y 200 caracteres")]
        public string Description { get; set; } = string.Empty;
    }

}
