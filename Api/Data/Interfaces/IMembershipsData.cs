using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IMembershipsData : IRepositoryData<Memberships>
    {
        public Task<IEnumerable<Memberships>> GetActiveMembershipsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Memberships>> GetMembershipsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Memberships>> GetMembershipsByTypeAsync(int membershipTypeId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Memberships>> GetMembershipsByVehicleIdAsync(int vehicleId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsVehicleWithActiveMembershipAsync(int vehicleId)
        {
            throw new NotImplementedException();
        }
    }
}
