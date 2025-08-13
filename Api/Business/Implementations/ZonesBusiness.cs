using AutoMapper;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dtos;
using Entity.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Implementations
{
 
    public class ZonesBusiness : RepositoryBusiness<Zones,ZonesDto>, IZonesBusiness
    {
        private readonly IZonesData _data;
        private readonly IMapper _mapper;
        private readonly ILogger<ZonesDto> _logger;
        public ZonesBusiness(IZonesData data, IMapper mapper, ILogger<ZonesDto> logger)
            : base(data, mapper)
        {
            _data = data;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<IEnumerable<ZonesDto>> GetAllJoinAsync()
        {
            var entities = await _data.GetAllJoinAsync();
            return _mapper.Map<IEnumerable<ZonesDto>>(entities);
        }
    }
}
