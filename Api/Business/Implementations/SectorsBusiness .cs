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
   
    public class SectorsBusiness : RepositoryBusiness<Sectors, SectorsDto>, ISectorsBusiness
    {
        private readonly ISectorsBusiness _data;
        public SectorsBusiness(ISectorsBusiness data, IMapper mapper)
            : base(data, mapper)
        {
            _data = data;
        }

    }
}
