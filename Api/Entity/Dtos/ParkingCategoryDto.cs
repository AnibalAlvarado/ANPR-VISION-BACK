using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos
{
    public class ParkingCategoryDto : GenericDto
    {
        [Required(ErrorMessage = "El código de la categoría es obligatorio.")]
        [StringLength(20, ErrorMessage = "El código no puede superar los 20 caracteres.")]
        public string? code { get; set; }

        [Required(ErrorMessage = "La descripción de la categoría es obligatoria.")]
        [StringLength(150, ErrorMessage = "La descripción no puede superar los 150 caracteres.")]
        public string? Description { get; set; }
    }
}
