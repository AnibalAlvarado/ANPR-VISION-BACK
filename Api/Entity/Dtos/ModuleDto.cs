using Entity.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos
{
    public class ModuleDto : GenericDto
    {
        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "La descripción debe tener entre 3 y 150 caracteres.")]
        public string Description { get; set; }
    }
}