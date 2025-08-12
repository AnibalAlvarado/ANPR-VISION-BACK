using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Business.Interfaces;
using Entity.Dtos;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientBusiness _clientBusiness;
        private readonly ILogger<ClientController> _logger;

        public ClientController(IClientBusiness clientBusiness, ILogger<ClientController> logger)
        {
            _clientBusiness = clientBusiness;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clients = await _clientBusiness.GetAllAsync();
            return Ok(clients);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var client = await _clientBusiness.GetByIdAsync(id);
            if (client == null)
                return NotFound(new { message = $"No se encontró el cliente con Id {id}" });

            return Ok(client);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClientDto clientDto)
        {
            if (clientDto == null)
                return BadRequest(new { message = "Datos inválidos" });

            var createdClient = await _clientBusiness.CreateAsync(clientDto);
            return Ok(createdClient); 
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ClientDto clientDto)
        {
            if (clientDto == null)
                return BadRequest(new { message = "Datos inválidos" });

            var updatedClient = await _clientBusiness.UpdateAsync(id, clientDto);
            return Ok(updatedClient);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _clientBusiness.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { message = $"No se encontró el cliente con Id {id}" });

            return NoContent();
        }
    }
}
