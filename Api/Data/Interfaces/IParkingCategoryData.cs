using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IParkingCategoryData : IRepositoryData<ParkingCategory>
    {
        public Task<int> CreateAsync(ParkingCategory parkingCategory)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ParkingCategory>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ParkingCategory?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(int id, ParkingCategory parkingCategory)
        {
            throw new NotImplementedException();
        }
    }
}
