using AutoMapper;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dtos;
using Entity.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ClientBusiness : IClientBusiness
    {
        private readonly IClientData _clientData;
        private readonly ILogger<ClientBusiness> _logger;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public ClientBusiness(
            IClientData clientData,
            ILogger<ClientBusiness> logger,
            IMapper mapper,
            IConfiguration configuration)
        {
            _clientData = clientData;
            _logger = logger;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<ClientDto> CreateAsync(ClientDto clientDto)
        {
            var entity = _mapper.Map<Client>(clientDto);
            var created = await _clientData.CreateAsync(entity);
            return _mapper.Map<ClientDto>(created);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _clientData.DeleteAsync(id);
        }

        public async Task<IEnumerable<ClientDto>> GetAllAsync()
        {
            var clients = await _clientData.GetAllAsync();
            return _mapper.Map<IEnumerable<ClientDto>>(clients);
        }

        public async Task<ClientDto> GetByIdAsync(int id)
        {
            var client = await _clientData.GetByIdAsync(id);
            if (client == null)
                throw new KeyNotFoundException($"No se encontró el cliente con Id {id}");

            return _mapper.Map<ClientDto>(client);
        }

        public async Task<ClientDto> UpdateAsync(int id, ClientDto clientDto)
        {
            var entity = _mapper.Map<Client>(clientDto);
            var updated = await _clientData.UpdateAsync(id, entity);
            return _mapper.Map<ClientDto>(updated);
        }
    }
}
