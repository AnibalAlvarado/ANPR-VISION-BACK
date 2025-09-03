using Entity.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos
{
    public class RolFormPermissionDto : BaseDto
    {
        [Required(ErrorMessage = "El Id del rol es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El Id del rol debe ser mayor que 0.")]
        public int RolId { get; set; }

    
        public string? RolName { get; set; } = null!;

        [Required(ErrorMessage = "El Id del permiso es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El Id del permiso debe ser mayor que 0.")]
        public int PermissionId { get; set; }

    
        public string? PermissionName { get; set; } = null!;

        [Required(ErrorMessage = "El Id del formulario es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El Id del formulario debe ser mayor que 0.")]
        public int FormId { get; set; }

        public string? FormName { get; set; } = null!;
    }
}
