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
        public Task<IEnumerable<Zones>> GetAllWithParkingAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Zones>> GetAllWithSectorsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Zones> GetByIdWithParkingAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Zones> GetByIdWithSectorsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Zones>> GetByParkingIdAsync(int parkingId)
        {
            throw new NotImplementedException();
        }
    }
}
