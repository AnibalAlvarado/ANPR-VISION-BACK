using Entity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos
{
    public class CameraDto : GenericDto
    {
        [Required(ErrorMessage = "La resolución es obligatoria")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "La resolución debe tener entre 5 y 50 caracteres")]
        public string Resolution { get; set; } = string.Empty;

        [Required(ErrorMessage = "La URL de la cámara es obligatoria")]
        [Url(ErrorMessage = "Debe ingresar una URL válida")]
        [StringLength(200, ErrorMessage = "La URL no puede superar los 200 caracteres")]
        public string Url { get; set; } = string.Empty;

        [Required(ErrorMessage = "El ID del parqueadero es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del parqueadero debe ser mayor a 0")]
        public int ParkingId { get; set; }


        public string? Parking { get; set; }
    }
}
