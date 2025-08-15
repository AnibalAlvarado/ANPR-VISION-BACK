using System;

namespace Entity.NewDtos
{
    public class MemberShipTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal PriceBase { get; set; }
        public int DurationDaysBase { get; set; }
        public bool Asset { get; set; }
        public bool? IsDeleted { get; set; }
    }
}