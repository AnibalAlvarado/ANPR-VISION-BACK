using Business.Interfaces.Security;
using Entity.Dtos.Security;
using Entity.Models.Security;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Implementations.Security
{
    [ApiController]
    [Route("api/[controller]")]

    public class RolController : RepositoryController<Rol, RolDto>
    {
      
        public RolController(IRolBusiness business )
            : base(business)
        {
        }
    }

    //public class RolController : RepositoryController<Rol, RolDto>
    //{
    //    public RolController(IRepositoryBusiness<Rol, RolDto> business) : base(business)
    //    {
    //    }
    //}
}
