using Business.Interfaces;
using Entity.Dtos;
using Entity.Models;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Implementations
{
    [ApiController]
    [Route("api/[controller]")]
    public class FormController : RepositoryController<Form, FormDto>
    {
        public FormController(IFormBusiness business)
            : base(business)
        {
        }
    }
}
