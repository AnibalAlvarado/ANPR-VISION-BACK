using Business.Interfaces;
using Entity.Dtos;
using Entity.Models;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Implementations
{
    [ApiController]
    [Route("api/[controller]")]
    public class SlotsController : RepositoryController<Slots, SlotsDto>
    {
        public SlotsController(ISlotsBusiness business)
            : base(business)
        {
        }
    }
}
