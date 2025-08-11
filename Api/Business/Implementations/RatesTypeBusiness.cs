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

    public class RatesTypeBusiness : RepositoryBusiness<RatesType, RatesTypeDto>, IRatesTypeBusiness
    {
        private readonly RatesTypeBusiness _data;
        public RatesTypeBusiness(RatesTypeBusiness data, IMapper mapper)
            : base(data, mapper)
        {
            _data = data;
        }

    }
}
