using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Dtos.Security;

namespace Business.Interfaces.Security.Authentication
{
    public interface IUserAuthenticationBusiness
    {
        Task<UserResponseDto?> AuthenticateAsync(string username, string password);
    }
}
