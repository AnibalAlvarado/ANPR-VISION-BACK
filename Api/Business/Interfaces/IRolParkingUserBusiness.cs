using Entity.Dtos;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IRolParkingUserBusiness : IRepositoryBusiness<RolParkingUser, RolParkingUserDto>
    {
        public Task<IEnumerable<RolParkingUserDto>> GetAllJoinAsync();
        Task<bool> ExistsAsync(int userId, int roleId);
    }
}
