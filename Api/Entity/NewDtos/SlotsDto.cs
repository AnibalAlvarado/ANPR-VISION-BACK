using System;

namespace Entity.NewDtos
{
    public class SlotsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAvailable { get; set; }
        public int SectorsId { get; set; }
        public bool Asset { get; set; }
        public bool? IsDeleted { get; set; }
    }
}