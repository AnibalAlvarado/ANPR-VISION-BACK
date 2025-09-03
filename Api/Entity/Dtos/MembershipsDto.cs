using Entity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos
{
    public class MembershipsDto : BaseDto
    {
        [Required(ErrorMessage = "El ID del tipo de membresía es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del tipo de membresía debe ser mayor a 0")]
        public int MembershipTypeId { get; set; }

        [Required(ErrorMessage = "El ID del vehículo es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del vehículo debe ser mayor a 0")]
        public int VehicleId { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        [DataType(DataType.Date, ErrorMessage = "La fecha de inicio no es válida")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "La fecha de fin es obligatoria")]
        [DataType(DataType.Date, ErrorMessage = "La fecha de fin no es válida")]
        [CustomValidation(typeof(MembershipsDto), nameof(ValidateEndDate))]
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "El precio de compra es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal PriceAtPurchase { get; set; }

        [Required(ErrorMessage = "La duración de la membresía es obligatoria")]
        [Range(1, 3650, ErrorMessage = "La duración debe estar entre 1 y 3650 días")]
        public int DurationDays { get; set; }

        [StringLength(3, ErrorMessage = "La moneda debe tener un máximo de 3 caracteres")]
        public string? Currency { get; set; }

        public string? MembershipType { get; set; }

        public string? Vehicle { get; set; }

        public static ValidationResult? ValidateEndDate(DateTime? endDate, ValidationContext context)
        {
            var instance = context.ObjectInstance as MembershipsDto;

            if (endDate.HasValue && instance?.StartDate.HasValue == true)
            {
                if (endDate.Value < instance.StartDate.Value)
                {
                    return new ValidationResult("La fecha de fin no puede ser anterior a la fecha de inicio");
                }
            }

            return ValidationResult.Success;
        }
    }
}
