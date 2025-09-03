using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos
{
    public class BlackListDto : BaseDto
    {
        [Required(ErrorMessage = "La razón de la restricción es obligatoria")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "La razón debe tener entre 5 y 200 caracteres")]
        public string Reason { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de restricción es obligatoria")]
        [DataType(DataType.Date, ErrorMessage = "La fecha de restricción no es válida")]
        [CustomValidation(typeof(BlackListDto), nameof(ValidateRestrictionDate))]
        public DateTime? RestrictionDate { get; set; }

        [Required(ErrorMessage = "El ID del vehículo es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del vehículo debe ser mayor a 0")]
        public int VehicleId { get; set; }

  
        public string? Vehicle { get; set; }

        public static ValidationResult? ValidateRestrictionDate(DateTime? restrictionDate, ValidationContext context)
        {
            if (restrictionDate.HasValue && restrictionDate.Value < DateTime.Today)
            {
                return new ValidationResult("La fecha de restricción no puede ser anterior a hoy");
            }

            return ValidationResult.Success;
        }
    }
}

