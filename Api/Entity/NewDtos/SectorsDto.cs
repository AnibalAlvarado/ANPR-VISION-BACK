using System;

namespace Entity.NewDtos
{
    public class SectorsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public int ZonesId { get; set; }
        public int TypeVehicleId { get; set; }
        public bool Asset { get; set; }
        public bool? IsDeleted { get; set; }
    }
}