using Entity.Dtos;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ISlotsBusiness : IRepositoryBusiness<Slots, SlotsDto>
    {

        public Task<SlotsDto> CreateAsync(SlotsDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SlotsDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<SlotsDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<SlotsDto> UpdateAsync(int id, SlotsDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
