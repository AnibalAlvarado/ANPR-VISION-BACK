using System;

namespace Entity.NewDtos
{
    public class MembershipsDto
    {
        public int Id { get; set; }
        public int MembershipTypeId { get; set; }
        public int VehicleId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal PriceAtPurchase { get; set; }
        public int DurationDays { get; set; }
        public string? Currency { get; set; }
        public bool Asset { get; set; }
        public bool? IsDeleted { get; set; }
    }
}