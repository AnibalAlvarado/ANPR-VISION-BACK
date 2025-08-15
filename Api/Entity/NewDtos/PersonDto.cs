using System;

namespace Entity.NewDtos
{
    public class PersonDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Document { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public bool Asset { get; set; }
        public bool? IsDeleted { get; set; }
    }
}