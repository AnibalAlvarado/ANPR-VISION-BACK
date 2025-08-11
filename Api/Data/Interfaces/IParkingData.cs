using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IParkingData : IRepositoryData<Parking>
    {

        public Task<int> CreateAsync(Parking parking)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Parking>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Parking?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(int id, Parking parking)
        {
            throw new NotImplementedException();
        }
    }
}
