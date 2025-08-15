using System;

namespace Entity.NewDtos
{
    public class RegisteredVehiclesDto
    {
        public int Id { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime? ExitDate { get; set; }
        public int VehicleId { get; set; }
        public int? SlotsId { get; set; }
        public bool Asset { get; set; }
        public bool? IsDeleted { get; set; }
    }
}