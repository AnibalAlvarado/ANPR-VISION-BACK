using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos
{
    public class MemberShipTypeDto : GenericDto
    {
        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(100, ErrorMessage = "La descripción no puede superar los 100 caracteres.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "El precio base es obligatorio.")]
        [Range(0.01, 999999.99, ErrorMessage = "El precio base debe ser mayor a 0 y menor a 1,000,000.")]
        public decimal PriceBase { get; set; }

        [Required(ErrorMessage = "La duración base es obligatoria.")]
        [Range(1, 3650, ErrorMessage = "La duración debe estar entre 1 y 3650 días.")]
        public int DurationDaysBase { get; set; }
    }
}
