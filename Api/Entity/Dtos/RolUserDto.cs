using Entity.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos
{
    public class RolUserDto : BaseDto
    {
        [Required(ErrorMessage = "El Id del usuario es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El Id del usuario debe ser mayor a 0.")]
        public int UserId { get; set; }

       
        public string? UserName { get; set; } = null!;

        [Required(ErrorMessage = "El Id del rol es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El Id del rol debe ser mayor a 0.")]
        public int RolId { get; set; }

        public string? RolName { get; set; } = null!;
    }


}
