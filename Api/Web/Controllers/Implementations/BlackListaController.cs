using Business.Interfaces;
using Entity.Dtos;
using Entity.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Utilities.Exceptions;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlackListController : ControllerBase
    {
        private readonly IBlackListBusiness _blackListBusiness;
        private readonly ILogger<BlackListController> _logger;

        public BlackListController(IBlackListBusiness blackListBusiness, ILogger<BlackListController> logger)
        {
            _blackListBusiness = blackListBusiness;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            try
            {
                var result = await _blackListBusiness.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los registros de la lista negra.");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetByIdAsync(int id)
        {
            try
            {
                var result = await _blackListBusiness.GetByIdAsync(id);
                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Registro no encontrado.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el registro de la lista negra por ID.");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync([FromBody] BlackListDto dto)
        {
            try
            {
                var newId = await _blackListBusiness.CreateAsync(dto);
                return CreatedAtAction(nameof(GetByIdAsync), new { id = newId }, null);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Error de validación al crear.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el registro de la lista negra.");
                return StatusCode(500, "Error interno del servidor");
            }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(int id, [FromBody] BlackListDto dto)
        {
            try
            {
                await _blackListBusiness.UpdateAsync(id, dto);
                return NoContent();
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Error de validación al actualizar.");
                return BadRequest(ex.Message);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Registro no encontrado al actualizar.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el registro de la lista negra.");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                await _blackListBusiness.DeleteAsync(id);
                return NoContent();
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Registro no encontrado al eliminar.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el registro de la lista negra.");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}
