using Entity.Dtos;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IParkingBusiness : IRepositoryBusiness<Parking, ParkingDto>
    {
        public Task<ParkingDto> CreateAsync(ParkingDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ParkingDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ParkingDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(int id, ParkingDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
