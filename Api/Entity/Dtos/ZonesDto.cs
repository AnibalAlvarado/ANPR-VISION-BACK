using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos
{
    public class ZonesDto : GenericDto
    {
        [Required(ErrorMessage = "El Id del parqueadero es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El Id del parqueadero debe ser mayor a 0.")]
        public int ParkingId { get; set; }


        public string? Parking { get; set; }
    }
}
