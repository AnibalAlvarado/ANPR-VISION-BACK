using Business.Interfaces;
using Entity.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleBusiness _vehicleBusiness;
        private readonly ILogger<VehicleController> _logger;

        public VehicleController(IVehicleBusiness vehicleBusiness, ILogger<VehicleController> logger)
        {
            _vehicleBusiness = vehicleBusiness;
            _logger = logger;
        }

        // GET: api/vehicle
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleDto>>> GetAll()
        {
            try
            {
                var vehicles = await _vehicleBusiness.GetAllAsync();
                return Ok(vehicles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los vehículos");
                return StatusCode(500, "Ocurrió un error al obtener los vehículos");
            }
        }

        // GET: api/vehicle/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleDto>> GetById(int id)
        {
            try
            {
                var vehicle = await _vehicleBusiness.GetByIdAsync(id);
                if (vehicle == null)
                    return NotFound($"No se encontró un vehículo con Id {id}");

                return Ok(vehicle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener el vehículo con Id {id}");
                return StatusCode(500, "Ocurrió un error al obtener el vehículo");
            }
        }

        // POST: api/vehicle
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] VehicleDto dto)
        {
            if (dto == null)
                return BadRequest("Los datos del vehículo son inválidos");

            try
            {
                var newId = await _vehicleBusiness.AddAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = newId }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el vehículo");
                return StatusCode(500, "Ocurrió un error al crear el vehículo");
            }
        }

        // PUT: api/vehicle/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] VehicleDto dto)
        {
            if (dto == null)
                return BadRequest("Los datos del vehículo son inválidos");

            try
            {
                await _vehicleBusiness.UpdateAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"No se encontró un vehículo con Id {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar el vehículo con Id {id}");
                return StatusCode(500, "Ocurrió un error al actualizar el vehículo");
            }
        }

        // DELETE: api/vehicle/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _vehicleBusiness.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"No se encontró un vehículo con Id {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar el vehículo con Id {id}");
                return StatusCode(500, "Ocurrió un error al eliminar el vehículo");
            }
        }
    }
}
