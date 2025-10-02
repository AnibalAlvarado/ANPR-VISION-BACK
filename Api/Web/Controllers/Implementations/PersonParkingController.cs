using Business.Interfaces;
using Entity.Dtos;
using Entity.Models;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Implementations
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonParkingController: RepositoryController<PersonParking, PersonParkingDto>
    {
        private readonly IPersonParkingBusiness _business;
        public PersonParkingController(IPersonParkingBusiness business)
            : base(business)
        {
            _business = business;
        }

        [HttpGet("join")]
        public async Task<ActionResult<IEnumerable<PersonParkingDto>>> GetAllJoin()
        {
            try
            {
                var data = await _business.GetAllJoinAsync();
                if (data == null || !data.Any())
                {
                    var responseNull = new ApiResponse<IEnumerable<PersonParkingDto>>(null, false, "Registro no encontrado", null);
                    return NotFound(responseNull);
                }
                var response = new ApiResponse<IEnumerable<PersonParkingDto>>(data, true, "Ok", null);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<IEnumerable<PersonParkingDto>>(null, false, ex.Message.ToString(), null);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet("by-person/{personId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ParkingDto>>>> GetParkingsByPerson(int personId)
        {
            try
            {
                var data = await _business.GetParkingsByPersonIdAsync(personId);

                if (data == null || !data.Any())
                {
                    var responseNull = new ApiResponse<IEnumerable<ParkingDto>>(null, false, "No se encontraron parqueaderos para esta persona", null);
                    return NotFound(responseNull);
                }

                var response = new ApiResponse<IEnumerable<ParkingDto>>(data, true, "Ok", null);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<IEnumerable<ParkingDto>>(null, false, ex.Message, null);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
