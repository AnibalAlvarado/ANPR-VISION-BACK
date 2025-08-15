using System;

namespace Entity.NewDtos
{
    public class ParkingDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int ParkingCategoryId { get; set; }
        public bool Asset { get; set; }
        public bool? IsDeleted { get; set; }
    }
}