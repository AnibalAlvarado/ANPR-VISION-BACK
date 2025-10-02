using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IRolParkingUserData : IRepositoryData<RolParkingUser>
    {
        public Task<IEnumerable<RolParkingUser>> GetAllJoinAsync();

        Task<bool> ExistsAsync(int userId, int roleId);
    }
}
