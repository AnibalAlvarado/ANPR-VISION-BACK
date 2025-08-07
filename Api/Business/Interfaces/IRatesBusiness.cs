using Entity.Dtos;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IRatesBusiness : IRepositoryBusiness<Rates, RatesDto>
    {

        public Task<RatesDto> CreateAsync(RatesDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RatesDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<RatesDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<RatesDto> UpdateAsync(int id, RatesDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
