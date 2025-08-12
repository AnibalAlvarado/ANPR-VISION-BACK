using Entity.Dtos;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IUserData : IRepositoryData<User>
    {
        Task<User?> GetUserByUsernameAsync(string username);

        Task<List<string>> GetUserRoleAsync(int userId);

        Task<List<UserRoleStatusDto>> GetUserRolesAsync(int userId);

    }
}
