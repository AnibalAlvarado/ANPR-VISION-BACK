using AutoMapper;
using Business.Interfaces;
using Data.Implementations;
using Data.Interfaces;
using Entity.Dtos;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Implementations
{
   
    public class RegisteredVehicleBusiness : RepositoryBusiness<RegisteredVehicles, RegisteredVehiclesDto>, IRegisteredVehicleBusiness
    {
        private readonly IRegisteredVehiclesData _data;
        public RegisteredVehicleBusiness(IRegisteredVehiclesData data, IMapper mapper)
            : base(data, mapper)
        {
            _data = data;
        }

    }
}
