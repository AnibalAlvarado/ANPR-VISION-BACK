using AutoMapper;
using Data.Interfaces;
using Entity.Contexts;
using Entity.Dtos;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Utilities.Audit.Services;
using Utilities.Interfaces;

namespace Data.Implementations
{
    public class ZonesData : RepositoryData<Zones>, IZonesData
    {
        public ZonesData(ApplicationDbContext context, IConfiguration configuration, IAuditService auditService, ICurrentUserService currentUserService, IMapper mapper)
            : base(context, configuration, auditService, currentUserService, mapper)
        {

        }

        public async Task<IEnumerable<ZonesDto>> GetAllJoinAsync()
        {
            return await _context.Zones
                .AsNoTracking()
                .Select(p => new ZonesDto
                {
                    // --- BaseDto ---
                    Id = p.Id,                      // int? en BaseDto
                    Asset = p.Asset,                 // bool? en BaseDto
                    IsDeleted = p.IsDeleted,         // bool en BaseDto

                    // --- GenericDto ---
                    Name = p.Name,                   // string en GenericDto

                    // --- ZonesDto ---
                    ParkingId = p.ParkingId,
                    Parking = p.Parking != null
                        ? p.Parking.Name
                        : null
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<Zones>> GetAllByParkingId(int parkingId)
        {
            return await _context.Zones
                .AsNoTracking()
                .Where(s => s.ParkingId == parkingId && s.IsDeleted == false)
                .ToListAsync();
        }

    }
}
