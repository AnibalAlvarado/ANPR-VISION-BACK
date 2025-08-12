using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public  interface IBlackListData : IRepositoryData<BlackList>
    {
        public Task<IEnumerable<BlackList>> GetByReasonAsync(string reason)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BlackList>> GetByRestrictionDateAsync(DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BlackList>> GetByVehicleIdAsync(int vehicleId)
        {
            throw new NotImplementedException();
        }
    }
}
