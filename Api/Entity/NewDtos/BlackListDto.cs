using System;

namespace Entity.NewDtos
{
    public class BlackListDto
    {
        public int Id { get; set; }
        public string Reason { get; set; }
        public DateTime RestrictionDate { get; set; }
        public int VehicleId { get; set; }
        public bool Asset { get; set; }
        public bool? IsDeleted { get; set; }
    }
}