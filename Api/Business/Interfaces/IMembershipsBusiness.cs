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
    public interface IMembershipsBusiness : IRepositoryBusiness<Memberships, MembershipsDto>
    {

        public Task<MembershipsDto> CreateAsync(MembershipsDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<MembershipsDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<MembershipsDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(int id, MembershipsDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
