using Business.Interfaces;
using Entity.Dtos;
using Entity.Models;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Implementations
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParkingCategoryController : RepositoryController<ParkingCategory, ParkingCategoryDto>
    {
        public ParkingCategoryController(IParkingCategoryBusiness business)
            : base(business)
        {
        }
    }
}
