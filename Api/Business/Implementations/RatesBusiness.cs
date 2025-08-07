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

    public class RatesBusiness : RepositoryBusiness<Rates, RatesDto>, IRatesBusiness
    {
        private readonly IRatesBusiness _data;
        public RatesBusiness(IRatesBusiness data, IMapper mapper)
            : base(data, mapper)
        {
            _data = data;
        }

    }
}
