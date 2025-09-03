using Entity.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos
{
    public class FormModuleDto : BaseDto
    {
        [Required(ErrorMessage = "El ID del formulario es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del formulario debe ser mayor a 0")]
        public int FormId { get; set; }

        public string? FormName { get; set; } = null!;

        [Required(ErrorMessage = "El ID del módulo es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del módulo debe ser mayor a 0")]
        public int ModuleId { get; set; }

        public string? ModuleName { get; set; } = null!;
    }
}
