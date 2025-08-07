using Entity.Dtos;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IParkingCategoryBusiness : IRepositoryBusiness<ParkingCategory, ParkingCategoryDto>
    {

        public Task<ParkingCategoryDto> CreateAsync(ParkingCategoryDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ParkingCategoryDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ParkingCategoryDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(int id, ParkingCategoryDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
