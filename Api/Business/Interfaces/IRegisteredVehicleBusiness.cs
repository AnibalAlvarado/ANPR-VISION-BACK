using Data.Implementations;
using Entity.Dtos;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IRegisteredVehicleBusiness : IRepositoryBusiness<RegisteredVehicles, RegisteredVehiclesDto>
    {

        public Task<RegisteredVehiclesDto> CreateAsync(RegisteredVehiclesDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RegisteredVehiclesDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<RegisteredVehiclesDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<RegisteredVehiclesDto> UpdateAsync(int id, RegisteredVehiclesDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
