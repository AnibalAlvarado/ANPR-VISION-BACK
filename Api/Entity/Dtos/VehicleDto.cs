using Entity.Models;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos
{
    public class VehicleDto : BaseDto
    {
        [Required(ErrorMessage = "La placa es obligatoria.")]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "La placa debe tener entre 4 y 10 caracteres.")]
        public string Plate { get; set; } = string.Empty;

        [StringLength(30, ErrorMessage = "El color no puede superar los 30 caracteres.")]
        public string? Color { get; set; }

        [Required(ErrorMessage = "El Id del tipo de vehículo es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El Id del tipo de vehículo debe ser mayor a 0.")]
        public int TypeVehicleId { get; set; }

        [Required(ErrorMessage = "El Id del cliente es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El Id del cliente debe ser mayor a 0.")]
        public int ClientId { get; set; }

   
 
        public string? Client { get; set; }

 
        public string? TypeVehicle { get; set; }
    }
}
