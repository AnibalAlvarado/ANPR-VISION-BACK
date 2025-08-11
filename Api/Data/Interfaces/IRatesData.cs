using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IRatesData : IRepositoryData<Rates>
    {
        public Task<int> CreateAsync(Rates entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Rates>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Rates> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Rates entity)
        {
            throw new NotImplementedException();
        }
    }
}
