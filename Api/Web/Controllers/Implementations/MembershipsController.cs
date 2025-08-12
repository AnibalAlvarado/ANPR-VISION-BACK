using Business.Interfaces;
using Entity.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembershipsController : ControllerBase
    {
        private readonly IMembershipsBusiness _membershipsBusiness;
        private readonly ILogger<MembershipsController> _logger;

        public MembershipsController(IMembershipsBusiness membershipsBusiness, ILogger<MembershipsController> logger)
        {
            _membershipsBusiness = membershipsBusiness;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MembershipsDto>>> GetAll()
        {
            try
            {
                var memberships = await _membershipsBusiness.GetAllAsync();
                return Ok(memberships);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo todas las membresías");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MembershipsDto>> GetById(int id)
        {
            try
            {
                var membership = await _membershipsBusiness.GetByIdAsync(id);
                if (membership == null)
                    return NotFound("Membresía no encontrada");

                return Ok(membership);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error obteniendo membresía con ID {id}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(MembershipsDto dto)
        {
            try
            {
                await _membershipsBusiness.CreateAsync(dto);
                return Ok("Membresía creada correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando membresía");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, MembershipsDto dto)
        {
            try
            {
                var updatedMembership = await _membershipsBusiness.UpdateAsync(id, dto);

                if (updatedMembership == null)
                    return NotFound("Membresía no encontrada");

                return Ok(updatedMembership);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error actualizando membresía con ID {id}");
                return StatusCode(500, "Error interno del servidor");
            }
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _membershipsBusiness.DeleteAsync(id);
                if (!deleted)
                    return NotFound("Membresía no encontrada");

                return Ok("Membresía eliminada correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error eliminando membresía con ID {id}");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}
