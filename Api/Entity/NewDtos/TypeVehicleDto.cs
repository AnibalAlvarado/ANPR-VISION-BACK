using System;
using System.Collections.Generic;

namespace Entity.NewDtos
{
    public class TypeVehicleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Asset { get; set; }
        public bool? IsDeleted { get; set; }
    }
}