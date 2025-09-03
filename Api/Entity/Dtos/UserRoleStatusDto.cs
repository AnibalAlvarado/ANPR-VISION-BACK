using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos
{
    public class UserRoleStatusDto
    {
        [Required(ErrorMessage = "El Id del rol-usuario es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El Id del rol-usuario debe ser mayor a 0.")]
        public int RolUserId { get; set; }

        [Required(ErrorMessage = "El nombre del rol es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre del rol no puede superar los 100 caracteres.")]
        public string RoleName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El estado Asset es obligatorio.")]
        public bool Asset { get; set; }
    }

}
