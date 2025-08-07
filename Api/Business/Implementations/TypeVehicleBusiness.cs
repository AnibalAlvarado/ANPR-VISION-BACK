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
 
    public class TypeVehicleBusiness : RepositoryBusiness<TypeVehicle, TypeVehicleDto>, ITypeVehicleBusiness
    {
        private readonly ITypeVehicleBusiness _data;
        public TypeVehicleBusiness(ITypeVehicleBusiness data, IMapper mapper)
            : base(data, mapper)
        {
            _data = data;
        }

    }
}
