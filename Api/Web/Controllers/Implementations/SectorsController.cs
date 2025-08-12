using Business.Interfaces;
using Entity.Dtos;
using Entity.Models;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Implementations
{
    [ApiController]
    [Route("api/[controller]")]
    public class SectorsController : RepositoryController<Sectors, SectorsDto>
    {
        public SectorsController(ISectorsBusiness business)
            : base(business)
        {
        }
    }
}
