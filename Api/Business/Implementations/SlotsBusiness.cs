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

    public class SlotsBusiness : RepositoryBusiness<Slots, SlotsDto>, ISlotsBusiness
    {
        private readonly ISlotsBusiness _data;
        public SlotsBusiness(ISlotsBusiness data, IMapper mapper)
            : base(data, mapper)
        {
            _data = data;
        }

    }
}
