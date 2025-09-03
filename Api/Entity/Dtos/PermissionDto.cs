using Entity.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos
{
    public class PermissionDto : GenericDto
    {
        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(200, ErrorMessage = "La descripción no puede superar los 200 caracteres.")]
        public string Description { get; set; }
    }
}
