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
  
    public class ParkingBusiness : RepositoryBusiness<Parking, ParkingDto>, IParkingBusiness
    {
        private readonly IParkingData _data;
        private readonly IMapper _mapper;
        private readonly ILogger<ParkingDto> _logger;
        public ParkingBusiness(IParkingData data, IMapper mapper, ILogger<ParkingDto> logger)
            : base(data, mapper)
        {
            _data = data;
            _mapper = mapper;
            _logger = logger;
        }
        

    }
}
