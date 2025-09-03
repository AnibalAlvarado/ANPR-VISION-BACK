using Entity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos
{
    public class SectorsDto:GenericDto
    {

        [Required(ErrorMessage = "La capacidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La capacidad debe ser mayor a 0.")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "El Id de la zona es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El Id de la zona debe ser mayor a 0.")]
        public int ZonesId { get; set; }

        [Required(ErrorMessage = "El Id del tipo de vehículo es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El Id del tipo de vehículo debe ser mayor a 0.")]
        public int TypeVehicleId { get; set; }


        public string? Zones { get; set; }

     
        public string? TypeVehicle { get; set; }
    }
}
