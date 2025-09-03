using Entity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos
{
    public class SlotsDto:GenericDto
    {
        [Required(ErrorMessage = "El estado de disponibilidad es obligatorio.")]
        public bool IsAvailable { get; set; }

        [Required(ErrorMessage = "El Id del sector es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El Id del sector debe ser mayor a 0.")]
        public int SectorsId { get; set; }

        public string? Sectors { get; set; }
    }
}
