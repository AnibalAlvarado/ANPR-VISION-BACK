using Business.Interfaces;
using Entity.Dtos;
using Entity.Models;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Implementations
{
    [ApiController]
    [Route("api/[controller]")]
    public class TypeVehicleController : RepositoryController<TypeVehicle, TypeVehicleDto>
    {
        public TypeVehicleController(ITypeVehicleBusiness business)
            : base(business)
        {
        }
    }
}
