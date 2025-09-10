using Business.Interfaces;
using Entity.Dtos;
using Entity.Dtos.Dashboard;
using Entity.Models;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Implementations
{
    [ApiController]
    [Route("api/[controller]")]
    public class SlotsController : RepositoryController<Slots, SlotsDto>
    {
        private readonly ISlotsBusiness _business;
        public SlotsController(ISlotsBusiness business)
            : base(business)
        {
            _business = business;
        }

        [HttpGet("join")]
        public async Task<ActionResult<IEnumerable<SlotsDto>>> GetAllJoin()
        {
            try
            {
                var data = await _business.GetAllJoinAsync();
                if (data == null || !data.Any())
                {
                    var responseNull = new ApiResponse<IEnumerable<SlotsDto>>(null, false, "Registro no encontrado", null);
                    return NotFound(responseNull);
                }
                var response = new ApiResponse<IEnumerable<SlotsDto>>(data, true, "Ok", null);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<IEnumerable<SlotsDto>>(null, false, ex.Message, null);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        [HttpGet("by-sector/{sectorId}")]
        public async Task<ActionResult<IEnumerable<SlotsDto>>> GetAllBysectorsId([FromRoute] int sectorId)
        {
            try
            {
                IEnumerable<SlotsDto> data = await _business.GetAllBySectorId(sectorId);
                if (data == null || !data.Any())
                {
                    var responseNull = new ApiResponse<IEnumerable<SlotsDto>>(null, false, "Registro no encontrado", null);
                    return NotFound(responseNull);
                }
                var response = new ApiResponse<IEnumerable<SlotsDto>>(data, true, "Ok", null);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<IEnumerable<SlotsDto>>(null, false, ex.Message, null);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        // 👇 NUEVO: disponibilidad por sector (ideal para widget de un panel por sector)
        [HttpGet("availability/by-type/parking/{parkingId:int}")]
        public async Task<ActionResult<ApiResponse<List<SlotsAvailabilityByTypeDto>>>> GetAvailabilityByTypeInParking([FromRoute] int parkingId)
        {
            try
            {
                var data = await _business.GetAvailabilityByParkingGroupedByTypeAsync(parkingId);
                var response = new ApiResponse<List<SlotsAvailabilityByTypeDto>>(data, true, "Ok", null);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                var response = new ApiResponse<List<SlotsAvailabilityByTypeDto>>(null, false, ex.Message, null);
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<List<SlotsAvailabilityByTypeDto>>(null, false, ex.Message, null);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


    }
}
