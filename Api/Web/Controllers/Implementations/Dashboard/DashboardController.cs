using Business.Interfaces.Dashboard;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Implementations.Dashboard
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardBusiness _business;
        public DashboardController(IDashboardBusiness business) => _business = business;

        /// <summary>
        /// Retorna la cantidad de vehículos actualmente estacionados (ExitDate == null).
        /// </summary>
        [HttpGet("parked-now-count")]
        public async Task<IActionResult> GetParkedNowCount(CancellationToken ct)
        {
            var count = await _business.GetCurrentParkedCountAsync(ct);
            return Ok(new { status = true, data = count });
        }
    }
}
