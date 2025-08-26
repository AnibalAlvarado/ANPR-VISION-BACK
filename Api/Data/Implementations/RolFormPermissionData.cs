using AutoMapper;
using Data.Interfaces;
using Entity.Contexts;
using Entity.DtoSpecific.RolFormPermission;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Audit.Services;
using Utilities.Interfaces;

namespace Data.Implementations
{
    public class RolFormPermissionData : RepositoryData<RolFormPermission>, IRolFormPermissionData
    {
        public RolFormPermissionData(ApplicationDbContext context, IConfiguration configuration, IAuditService auditService, ICurrentUserService currentUserService, IMapper mapper)
            : base(context, configuration,auditService, currentUserService, mapper)
        {

        }

        public async Task<IEnumerable<RolFormPermission>> GetAllJoinAsync()
        {
            //await AuditAsync("GetAllJoinAsync");

            return await _context.RolFormPermission
                .Include(x => x.Rol)
                .Include(x => x.Form)
                .Include(x => x.Permission)
                .ToListAsync();
        }

        public async Task<RolFormPermissionGroupedDto?> GetAllByRolId(int rolId)
        {
            return await _context.RolFormPermission
                .AsNoTracking()
                .Where(x => x.RolId == rolId)
                .GroupBy(x => new { x.RolId, RolName = x.Rol.Name })
                .Select(rg => new RolFormPermissionGroupedDto
                {
                    RolId = rg.Key.RolId,
                    RolName = rg.Key.RolName,
                    Forms = rg.GroupBy(x => new { x.FormId, FormName = x.Form.Name })
                        .Select(fg => new FormPermissionDto
                        {
                            FormId = fg.Key.FormId,
                            FormName = fg.Key.FormName,
                            Permissions = fg.Select(p => p.Permission.Name).Distinct().ToList(),
                            Modules = fg.SelectMany(p => p.Form.FormModules)
                                        .Select(m => new ModuleDtoSpecific
                                        {
                                            Id = m.Module.Id,
                                            Name = m.Module.Name
                                        })
                                        .Distinct()
                                        .ToList()
                        }).ToList()
                })
                .FirstOrDefaultAsync();
        }


    }
}
