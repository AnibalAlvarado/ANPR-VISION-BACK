using Entity.Dtos;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface ISlotsData : IRepositoryData<Slots>
    {
       Task<IEnumerable<SlotsDto>> GetAllJoinAsync();
        Task<IEnumerable<Slots>> GetAllBySectorId(int sectorId);
    }
}
