using Business.Interfaces;
using Entity.Dtos;
using Entity.Models;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Implementations
{
    [ApiController]
    [Route("api/[controller]")]
    public class ZonesController : RepositoryController<Zones, ZonesDto>
    {
        public ZonesController(IZonesBusiness business)
            : base(business)
        {
        }
    }
}
