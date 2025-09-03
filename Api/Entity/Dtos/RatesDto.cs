using Entity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos
{
    public class RatesDto:GenericDto
    {
        [Required(ErrorMessage = "El tipo de tarifa es obligatorio.")]
        [StringLength(50, ErrorMessage = "El tipo de tarifa no puede superar los 50 caracteres.")]
        public string Type { get; set; } = string.Empty;

        [Required(ErrorMessage = "El monto es obligatorio.")]
        [Range(0.01, 999999.99, ErrorMessage = "El monto debe ser mayor que 0 y menor a 1,000,000.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "La hora de inicio es obligatoria.")]
        public DateTime? StarHour { get; set; }

        [Required(ErrorMessage = "La hora de fin es obligatoria.")]
        public DateTime? EndHour { get; set; }

        [Required(ErrorMessage = "El año es obligatorio.")]
        [Range(2000, 2100, ErrorMessage = "El año debe estar entre 2000 y 2100.")]
        public int? Year { get; set; }

        [Required(ErrorMessage = "El ID de parqueadero es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID de parqueadero debe ser mayor a 0.")]
        public int ParkingId { get; set; }

        [Required(ErrorMessage = "El ID del tipo de tarifa es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del tipo de tarifa debe ser mayor a 0.")]
        public int RatesTypeId { get; set; }

        [Required(ErrorMessage = "El ID del tipo de vehículo es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del tipo de vehículo debe ser mayor a 0.")]
        public int TypeVehicleId { get; set; }

        public string? RatesType { get; set; }

        public string? TypeVehicle { get; set; }

        public string? Parking { get; set; }

        // Validación personalizada: EndHour > StarHour
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StarHour.HasValue && EndHour.HasValue && EndHour <= StarHour)
            {
                yield return new ValidationResult(
                    "La hora de fin debe ser mayor que la hora de inicio.",
                    new[] { nameof(EndHour) });
            }
        }
    }
}
