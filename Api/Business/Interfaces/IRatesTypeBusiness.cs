using Entity.Dtos;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IRatesTypeBusiness : IRepositoryBusiness<RatesType, RatesTypeDto>
    {

        public Task<RatesTypeDto> CreateAsync(RatesTypeDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RatesTypeDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<RatesTypeDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<RatesTypeDto> UpdateAsync(int id, RatesTypeDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
