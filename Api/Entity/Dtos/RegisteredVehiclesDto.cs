using Entity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos
{
    public class RegisteredVehiclesDto : BaseDto
    {
        [Required(ErrorMessage = "La fecha de entrada es obligatoria.")]
        public DateTime? EntryDate { get; set; }

        [Required(ErrorMessage = "La fecha de salida es obligatoria.")]
        [DataType(DataType.DateTime, ErrorMessage = "La fecha de salida no es válida.")]
        public DateTime? ExitDate { get; set; }

        [Required(ErrorMessage = "El ID del vehículo es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un vehículo válido.")]
        public int VehicleId { get; set; }

        public string? Vehicle { get; set; }

        // Opcional
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un slot válido.")]
        public int? SlotsId { get; set; }

        public string? Slots { get; set; }
    }
}
