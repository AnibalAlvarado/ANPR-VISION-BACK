using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface ISectorsData : IRepositoryData<Sectors>
    {

        public Task<IEnumerable<Sectors>> GetAllWithZoneAndTypeAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Sectors>> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Sectors>> GetByTypeVehicleIdAsync(int typeVehicleId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Sectors>> GetByZoneIdAsync(int zoneId)
        {
            throw new NotImplementedException();
        }

        public Task<Sectors> GetWithDetailsByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
