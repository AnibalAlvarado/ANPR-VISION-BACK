using AutoMapper;
using Business.Interfaces;
using Entity.Dtos;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Implementations
{

    public class VehicleBusiness : RepositoryBusiness<Vehicle, VehicleDto> ,IVehicleBusiness
    {
        private readonly IVehicleBusiness _data;
        public VehicleBusiness(IVehicleBusiness data, IMapper mapper)
            : base(data, mapper)
        {
            _data = data;
        }

    }
}
