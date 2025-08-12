using Business.Interfaces;
using Entity.Dtos;
using Entity.Models;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Implementations
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatesTypeController : RepositoryController<RatesType, RatesTypeDto>
    {
        public RatesTypeController(IRatesTypeBusiness business)
            : base(business)
        {
        }
    }
}
