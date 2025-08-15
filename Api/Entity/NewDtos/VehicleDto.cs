using System;
using System.Collections.Generic;

namespace Entity.NewDtos
{
    public class VehicleDto
    {
        public int Id { get; set; }
        public string Plate { get; set; }
        public string? Color { get; set; }
        public int TypeVehicleId { get; set; }
        public int ClientId { get; set; }
        public bool Asset { get; set; }
        public bool? IsDeleted { get; set; }
    }
}