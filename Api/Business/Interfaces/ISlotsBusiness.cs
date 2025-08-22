﻿using Entity.Dtos;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ISlotsBusiness : IRepositoryBusiness<Slots, SlotsDto>
    {
        Task<IEnumerable<SlotsDto>> GetAllJoinAsync();
        Task<IEnumerable<SlotsDto>> GetAllBySectorId(int sectorId);
    }
}
