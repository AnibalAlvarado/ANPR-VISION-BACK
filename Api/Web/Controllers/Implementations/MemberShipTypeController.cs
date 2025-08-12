using Business.Interfaces;
using Entity.Dtos;
using Entity.Models;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Implementations
{
    [ApiController]
    [Route("api/[controller]")] 
    public class MemberShipTypeController : RepositoryController<MemberShipType, MemberShipTypeDto>
    {
        public MemberShipTypeController(IMemberShipTypeBusiness business)
            : base(business)
        {
        }
    }
}
