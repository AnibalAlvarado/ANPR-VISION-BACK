using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IZonesData : IRepositoryData<Zones>
    {
        Task<IEnumerable<Zones>> GetAllJoinAsync();


    }
}
