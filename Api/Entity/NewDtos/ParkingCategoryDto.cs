using System;

namespace Entity.NewDtos
{
    public class ParkingCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool Asset { get; set; }
        public bool? IsDeleted { get; set; }
    }
}