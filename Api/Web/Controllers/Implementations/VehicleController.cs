using AutoMapper;
using Business.Interfaces;
using Data.Implementations;
using Entity.Dtos;
using Entity.Models;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Implementations
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController: RepositoryController<Vehicle, VehicleDto>
    {
        private readonly IVehicleBusiness _business;
        private readonly IMapper _mapper;

        public VehicleController(IVehicleBusiness business, IMapper mapper)
            : base(business)
        {
            _business = business;
            _mapper = mapper;
        }

        [HttpPost]
        public override async Task<ActionResult<VehicleDto>> Save(VehicleDto dto)
        {
            try
            {
                // 1️⃣ Guardar el vehículo normalmente usando la lógica genérica
                VehicleDto dtoSaved = await _business.Save(dto);

                // 2️⃣ Asignar automáticamente un slot al vehículo recién creado
                var registeredVehicle = await _business.RegisterVehicleWithSlotAsync(dtoSaved.Id);
                var registeredVehicleDto = _mapper.Map<RegisteredVehiclesDto>(registeredVehicle);

                var response = new
                {
                    Vehicle = dtoSaved,
                    RegisteredVehicle = registeredVehicleDto
                };

                return CreatedAtRoute(new { id = dtoSaved.Id }, response);


                //// 3️⃣ Preparar la respuesta combinada
                //var response = new
                //{
                //    Vehicle = dtoSaved,
                //    RegisteredVehicle = registeredVehicle
                //};

                //return CreatedAtRoute(new { id = dtoSaved.Id }, response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<object>(null, false, ex.Message, null);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        [HttpGet("join")]
        public async Task<ActionResult<IEnumerable<VehicleDto>>> GetAllJoin()
        {
            try
            {
                var data = await _business.GetAllJoinAsync();
                if (data == null || !data.Any())
                {
                    var responseNull = new ApiResponse<IEnumerable<VehicleDto>>(null, false, "Registro no encontrado", null);
                    return NotFound(responseNull);
                }
                var response = new ApiResponse<IEnumerable<VehicleDto>>(data, true, "Ok", null);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<IEnumerable<VehicleDto>>(null, false, ex.Message.ToString(), null);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet("slot/{slotId}")]
        public async Task<IActionResult> GetActiveVehicleBySlot(int slotId)
        {
            var result = await _business.GetActiveVehicleBySlotAsync(slotId); 

            if (result == null)
                return NotFound("No hay un vehículo activo en este slot.");

            return Ok(result);
        }


    }
}
