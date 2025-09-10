using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos.Dashboard
{
    public sealed class SlotsAvailabilityByTypeDto
    {
        public int TypeVehicleId { get; set; }
        public string TypeVehicleName { get; set; } = string.Empty;

        public int Total { get; set; }
        public int Disponibles { get; set; }
        public int Ocupados => Total - Disponibles;

        public double PorcentajeOcupacion => Total == 0 ? 0 : Math.Round((double)Ocupados * 100.0 / Total, 2);
    }

}
