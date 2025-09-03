using Entity.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos
{
    public class PersonDto : BaseDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(50, ErrorMessage = "El apellido no puede superar los 50 caracteres.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El documento es obligatorio.")]
        [RegularExpression(@"^\d{6,15}$", ErrorMessage = "El documento debe contener entre 6 y 15 dígitos.")]
        public string Document { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [RegularExpression(@"^\+?\d{7,15}$", ErrorMessage = "Ingrese un teléfono válido, entre 7 y 15 dígitos.")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        [StringLength(100, ErrorMessage = "El correo no puede superar los 100 caracteres.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La edad es obligatoria.")]
        [Range(18, 120, ErrorMessage = "La edad debe estar entre 18 y 120 años.")]
        public int Age { get; set; }

    }
}
