using AutoMapper;
using Data.Interfaces.Operational;
using Entity.Contexts;
using Entity.Dtos.Operational;
using Entity.Models.Operational;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Audit.Services;
using Utilities.Interfaces;

namespace Data.Implementations.Operational
{
    public class BlackListData : RepositoryData<BlackList>, IBlackListData
    {
        public BlackListData(ApplicationDbContext context, IConfiguration configuration, IAuditService auditService, ICurrentUserService currentUserService, IMapper mapper)
            : base(context, configuration, auditService, currentUserService, mapper)
        {

        }

        public async Task<IEnumerable<BlackListDto>> GetAllJoinAsync()
        {
            return await _context.BlackList
                .AsNoTracking()
                .Select(p => new BlackListDto
                {
                    // --- BaseDto ---
                    Id = p.Id,                      // int? en BaseDto
                    Asset = p.Asset,                 // bool? en BaseDto
                    IsDeleted = p.IsDeleted,         // bool en BaseDto

                    // --- GenericDto ---

                    Reason = p.Reason,
                    RestrictionDate = p.RestrictionDate,


                     

                    // --- ZonesDto ---
                    VehicleId = p.VehicleId,
                    Vehicle = p.Vehicle != null  
                        ? p.Vehicle.Plate
                        : null
                })
                .ToListAsync();
        }

        
    }
}
