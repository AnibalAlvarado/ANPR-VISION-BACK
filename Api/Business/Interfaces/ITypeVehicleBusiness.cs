using Entity.Dtos;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ITypeVehicleBusiness : IRepositoryBusiness<TypeVehicle, TypeVehicleDto>
    {
        public Task<TypeVehicleDto> CreateAsync(TypeVehicleDto dto)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<TypeVehicleDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
        public Task<TypeVehicleDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<TypeVehicleDto> UpdateAsync(int id, TypeVehicleDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
