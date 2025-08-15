using System;

namespace Entity.NewDtos
{
    public class ZonesDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParkingId { get; set; }
        public bool Asset { get; set; }
        public bool? IsDeleted { get; set; }
    }
}