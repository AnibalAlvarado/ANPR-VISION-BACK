using Entity.Dtos;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ISectorsBusiness : IRepositoryBusiness<Sectors, SectorsDto>
    {

        public Task<SectorsDto> CreateAsync(SectorsDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SectorsDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<SectorsDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<SectorsDto> UpdateAsync(int id, SectorsDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
