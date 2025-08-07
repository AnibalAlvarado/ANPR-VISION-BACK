using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IRatesTypeData : IRepositoryData<RatesType>
    {

        public Task<int> CreateAsync(RatesType ratesType)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RatesType>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<RatesType?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(int id, RatesType ratesType)
        {
            throw new NotImplementedException();
        }
    }
}
