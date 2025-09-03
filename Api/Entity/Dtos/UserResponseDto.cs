using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos
{
    public class UserResponseDto
    {
        [Required(ErrorMessage = "El Id del usuario es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El Id del usuario debe ser mayor a 0.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre de usuario debe tener entre 3 y 50 caracteres.")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "La lista de roles es obligatoria.")]
        public List<string> Roles { get; set; } = new List<string>();

        [Required(ErrorMessage = "El token es obligatorio.")]
        public string Token { get; set; } = string.Empty;

        //public string Role { get; set; } = null!;
    }
}
