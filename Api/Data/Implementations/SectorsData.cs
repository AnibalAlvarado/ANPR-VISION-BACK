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
using System.Text;
using System.Threading.Tasks;
using Utilities.Audit.Services;
using Utilities.Interfaces;

namespace Data.Implementations
{
    public class SectorsData : RepositoryData<Sectors>, ISectorsData
    {
        public SectorsData(ApplicationDbContext context, IConfiguration configuration, IAuditService auditService, ICurrentUserService currentUserService, IMapper mapper)
            : base(context, configuration, auditService, currentUserService, mapper)
        {

        }

        public async Task<IEnumerable<SectorsDto>> GetAllJoinAsync()
        {
            return await _context.Sectors
                .AsNoTracking()
                .Select(p => new SectorsDto
                {
                    // --- BaseDto ---
                    Id = p.Id,                      // int? en BaseDto
                    Asset = p.Asset,                 // bool? en BaseDto
                    IsDeleted = p.IsDeleted,         // bool en BaseDto

                    // --- GenericDto ---
                    Name = p.Name,                   // string en GenericDto

                    // --- SectorsDto ---
                    Capacity = p.Capacity,
                    ZonesId = p.ZonesId,
                    Zones = p.Zones != null
                        ? p.Zones.Name
                        : null,
                    TypeVehicleId = p.TypeVehicleId,
                    TypeVehicle = p.TypeVehicle != null
                        ? p.TypeVehicle.Name
                        : null
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<Sectors>> GetAllByZoneId(int zoneId)
        {
            return await _context.Sectors
                .AsNoTracking()
                .Where(s => s.ZonesId == zoneId && s.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<List<Sectors>> GetSectorsByVehicleTypeAsync(int vehicleTypeId)
        {
            // Traer sectores que tengan el mismo tipo de vehículo
            // e incluir los slots relacionados
            var sectors = await _context.Sectors
                .Where(s => s.TypeVehicleId == vehicleTypeId)
                .Include(s => s.Slots)
                .ToListAsync();

            return sectors;
        }

    }
}
