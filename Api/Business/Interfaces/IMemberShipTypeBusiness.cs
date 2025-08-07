using Entity.Dtos;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IMemberShipTypeBusiness : IRepositoryBusiness<MemberShipType, MemberShipTypeDto>
    {

        public Task<MemberShipTypeDto> CreateAsync(MemberShipTypeDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<MemberShipTypeDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<MemberShipTypeDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(int id, MemberShipTypeDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
