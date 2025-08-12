using AutoMapper;
using Data.Interfaces;
using Entity.Contexts;
using Entity.Dtos;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Audit.Services;
using Utilities.Interfaces;

namespace Data.Implementations
{
    public class UserData : RepositoryData<User>, IUserData
    {
        private readonly ILogger<UserData> _logger;
        private readonly IAuditService _auditService;
        public UserData(ApplicationDbContext context, IConfiguration configuration,ILogger<UserData> logger, IAuditService auditService, ICurrentUserService currentUserService, IMapper mapper)
            : base(context, configuration, auditService, currentUserService, mapper)
        {
            _logger = logger;
            _auditService = auditService;
        }

        public override async Task<IEnumerable<User>> GetAll()
        {
            try
            {
                var users = await _context.Users
                    .Include(u => u.Person)
                    .ToListAsync();

                await AuditAsync("GetAll");

                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los usuarios con información de la persona");
                throw;
            }
        }

        public override async Task<User?> GetById(int id)
        {
            try
            {
                // Auditar acción GetById, enviamos la entidad si la encontró
                await AuditAsync("GetById", id);
                return await _context.Users
                    .Include(u => u.Person)
                    .FirstOrDefaultAsync(u => u.Id == id);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el usuario con ID {Id}", id);
                throw;
            }
        }


        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            try
            {
                await AuditAsync("GetUserByUsernameAsync");
                return await _context.Set<User>()
                    .FirstOrDefaultAsync(u => u.Username == username && u.Asset);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuario por nombre de usuario: {Username}", username);
                throw;
            }
        }

        //public async Task<string> GetUserRoleAsync(int userId)
        //{
        //    try
        //    {
        //        // Obtener el rol a través de la tabla pivote RolUser
        //        var userRole = await _context.Set<RolUser>()
        //            .Where(ru => ru.UserId == userId)
        //            .Join(
        //                _context.Set<Rol>(),
        //                ru => ru.RolId,
        //                r => r.Id,
        //                (ru, r) => new { RolName = r.Name }
        //            )
        //            .FirstOrDefaultAsync();
        //        await AuditAsync("GetUserRoleAsync", userId);

        //        return userRole?.RolName ?? "Guest";
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error al obtener rol para el usuario con ID: {UserId}", userId);
        //        throw;
        //    }
        //}


        public async Task<List<string>> GetUserRoleAsync(int userId)
        {
            try
            {
                // Obtener todos los roles asociados al usuario
                var userRoles = await _context.Set<RolUser>()
                    .Where(ru => ru.UserId == userId)
                    .Join(
                        _context.Set<Rol>(),
                        ru => ru.RolId,
                        r => r.Id,
                        (ru, r) => r.Name
                    )
                    .ToListAsync();

                await AuditAsync("GetUserRolesAsync", userId);

                // Si no tiene roles, devolver "Guest" como único rol
                return userRoles.Any() ? userRoles : new List<string> { "Guest" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener roles para el usuario con ID: {UserId}", userId);
                throw;
            }
        }


        public async Task<List<UserRoleStatusDto>> GetUserRolesAsync(int userId)
        {
            try
            {
                var userRoles = await _context.Set<RolUser>()
                    .Where(ru => ru.UserId == userId)
                    .Join(
                        _context.Set<Rol>(),
                        ru => ru.RolId,
                        r => r.Id,
                        (ru, r) => new UserRoleStatusDto
                        {
                            RolUserId = ru.Id,      // ID de la tabla pivote
                            RoleName = r.Name,
                            Asset = ru.Asset
                        }
                    )
                    .ToListAsync();

                await AuditAsync("GetUserRolesAsync", userId);

                return userRoles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener roles para el usuario con ID: {UserId}", userId);
                throw;
            }
        }



    }
}
