using Entity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Interfaces;

namespace Utilities.Implementations
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly PasswordHasher<User> _hasher = new();
        private readonly ILogger<PasswordHasher> _logger;
        private const int WorkFactor = 12; // ajustable por config

        public PasswordHasher(ILogger<PasswordHasher> logger) => _logger = logger;
        public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            var result = _hasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
            return result == PasswordVerificationResult.Success;
        }

        public string HashPassword(string password)
        {
            try
            {
                return BCrypt.Net.BCrypt.HashPassword(password, workFactor: WorkFactor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar hash de contraseña");
                throw;
            }
        }

        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error al verificar hash (posible cadena inválida)");
                return false;
            }
        }
    }
}
