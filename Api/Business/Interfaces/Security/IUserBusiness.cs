using Entity.Dtos.Security;
using Entity.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces.Security
{
    public interface IUserBusiness : IRepositoryBusiness<User, UserDto>
    {
        Task<UserResponseDto?> ValidateUserAsync(string username, string password);
        Task AssignDefaultRoleAsync(int userId);
        Task SendWelcomeEmailAsync(string to);

        Task<List<UserRoleStatusDto>> GetUserRolesAsync(int userId);


        // === Métodos para recuperación de contraseña ===
        Task RequestPasswordResetAsync(string email);
        Task VerifyCodeAndResetPasswordAsync(string email, string code, string newPassword);

        Task<bool> VerifyResetCodeAsync(string email, string code);



    }
}
