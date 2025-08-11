using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IRegisteredVehiclesData : IRepositoryData<RegisteredVehicles>
    {
        public Task AddAsync(RegisteredVehicles registeredVehicle)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RegisteredVehicles>> GetActiveVehiclesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RegisteredVehicles>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<RegisteredVehicles> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RegisteredVehicles>> GetByVehicleIdAsync(int vehicleId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(RegisteredVehicles registeredVehicle)
        {
            throw new NotImplementedException();
        }
    }
}
