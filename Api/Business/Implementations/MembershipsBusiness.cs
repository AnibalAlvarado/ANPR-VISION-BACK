using AutoMapper;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dtos;
using Entity.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Implementations
{
    public class MembershipsBusiness : IMembershipsBusiness
    {
        private readonly IMembershipsData _membershipsData;
        private readonly ILogger<MembershipsBusiness> _logger;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public MembershipsBusiness(
            IMembershipsData membershipsData,
            ILogger<MembershipsBusiness> logger,
            IMapper mapper,
            IConfiguration configuration)
        {
            _membershipsData = membershipsData;
            _logger = logger;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<IEnumerable<MembershipsDto>> GetAllAsync()
        {
            var entities = await _membershipsData.GetAllAsync();
            return _mapper.Map<IEnumerable<MembershipsDto>>(entities);
        }

        public async Task<MembershipsDto> GetByIdAsync(int id)
        {
            var entity = await _membershipsData.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"No se encontró la membresía con Id {id}");

            return _mapper.Map<MembershipsDto>(entity);
        }

        public async Task<MembershipsDto> CreateAsync(MembershipsDto dto)
        {
            var entity = _mapper.Map<Memberships>(dto);
            var created = await _membershipsData.CreateAsync(entity);
            return _mapper.Map<MembershipsDto>(created);
        }

        public async Task<MembershipsDto> UpdateAsync(int id, MembershipsDto dto)
        {
            var existingEntity = await _membershipsData.GetByIdAsync(id);
            if (existingEntity == null)
                return null;

            // Mapeamos dto a la entidad existente
            _mapper.Map(dto, existingEntity);

            var updated = await _membershipsData.UpdateAsync(existingEntity);
            return _mapper.Map<MembershipsDto>(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _membershipsData.DeleteAsync(id);
        }
    }
}
