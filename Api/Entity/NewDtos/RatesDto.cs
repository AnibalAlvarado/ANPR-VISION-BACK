using System;

namespace Entity.NewDtos
{
    public class RatesDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime StarHour { get; set; }
        public DateTime EndHour { get; set; }
        public int Year { get; set; }
        public int ParkingId { get; set; }
        public int RatesTypeId { get; set; }
        public int TypeVehicleId { get; set; }
        public bool Asset { get; set; }
        public bool? IsDeleted { get; set; }
    }
}