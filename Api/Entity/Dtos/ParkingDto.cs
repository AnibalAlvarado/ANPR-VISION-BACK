
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos
{
    public class ParkingDto : GenericDto
    {
        [Required(ErrorMessage = "La ubicación es obligatoria.")]
        [StringLength(150, ErrorMessage = "La ubicación no puede superar los 150 caracteres.")]
        public string? Location { get; set; }

        [Required(ErrorMessage = "La categoría del parqueadero es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoría de parqueadero válida.")]
        public int ParkingCategoryId { get; set; }

        public string? ParkingCategory { get; set; }
    }
}
