using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IMemberShipTypeData : IRepositoryData<MemberShipType>
    {

        public Task<IEnumerable<MemberShipType>> GetAllWithMembershipsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<MemberShipType> GetByIdWithMembershipsAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
