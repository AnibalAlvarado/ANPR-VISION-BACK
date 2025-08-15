using System;

namespace Entity.NewDtos
{
    public class CameraDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Resolution { get; set; }
        public string Url { get; set; }
        public int ParkingId { get; set; }
        public bool Asset { get; set; }
        public bool? IsDeleted { get; set; }
    }
}