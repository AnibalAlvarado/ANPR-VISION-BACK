using Business.Interfaces;
using Entity.Dtos;
using Entity.Models;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Implementations
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : RepositoryController<Person, PersonDto>
    {
        public PersonController(IPersonBusiness business)
            : base(business)
        {
        }
    }
}
