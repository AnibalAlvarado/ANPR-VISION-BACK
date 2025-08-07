using Entity.Dtos;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IZonesBusiness : IRepositoryBusiness<Zones, ZonesDto>
    {
        public Task<ZonesDto> CreateAsync(ZonesDto dto)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<ZonesDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
        public Task<ZonesDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<ZonesDto> UpdateAsync(int id, ZonesDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
