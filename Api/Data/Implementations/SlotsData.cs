using AutoMapper;
using Data.Interfaces;
using Entity.Contexts;
using Entity.Dtos;
using Entity.Dtos.Dashboard;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Utilities.Audit.Services;
using Utilities.Interfaces;

namespace Data.Implementations
{
    public class SlotsData : RepositoryData<Slots>, ISlotsData
    {
        public SlotsData(ApplicationDbContext context, IConfiguration configuration, IAuditService auditService, ICurrentUserService currentUserService, IMapper mapper)
            : base(context, configuration, auditService, currentUserService, mapper)
        {

        }

        public async Task<IEnumerable<SlotsDto>> GetAllJoinAsync()
        {
            return await _context.Slots
                .AsNoTracking()
                .Select(p => new SlotsDto
                {
                    // --- BaseDto ---
                    Id = p.Id,                      // int? en BaseDto
                    Asset = p.Asset,                 // bool? en BaseDto
                    IsDeleted = p.IsDeleted,         // bool en BaseDto

                    // --- GenericDto ---
                    Name = p.Name,                   // string en GenericDto

                    // --- SectorsDto ---
                    IsAvailable = p.IsAvailable,
                    SectorsId = p.SectorsId,
                    Sectors = p.Sectors != null
                        ? p.Sectors.Name
                        : null
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<Slots>> GetAllBySectorId(int sectorId)
        {
            return await _context.Slots
                .AsNoTracking()
                .Where(s => s.SectorsId == sectorId && s.IsDeleted == false)
                .ToListAsync();
        }

        public Task<bool> ExistsAsync<T>(Func<object, bool> value)
        {
            throw new NotImplementedException();
        }

        public Task GetByIdAsync<T>(int sectorsId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SlotsAvailabilityByTypeDto>> GetAvailabilityByParkingGroupedByTypeAsync(int parkingId)
        {
            // Usa navegaciones y tolera IsDeleted nullable en todas las tablas
            var query = _context.Slots
                .AsNoTracking()
                .Where(s =>
                    s.Sectors != null &&
                    s.Sectors.Zones != null &&
                    s.Sectors.Zones.ParkingId == parkingId &&
                    s.IsDeleted != true &&                 // en lugar de == false
                    s.Sectors.IsDeleted != true &&
                    s.Sectors.Zones.IsDeleted != true
                )
                .GroupBy(s => new
                {
                    TypeVehicleId = s.Sectors.TypeVehicleId,
                    TypeVehicleName = s.Sectors.TypeVehicle != null ? s.Sectors.TypeVehicle.Name : ""
                })
                .Select(g => new SlotsAvailabilityByTypeDto
                {
                    TypeVehicleId = g.Key.TypeVehicleId,
                    TypeVehicleName = g.Key.TypeVehicleName,
                    Total = g.Count(),
                    Disponibles = g.Count(x => x.IsAvailable)
                })
                .OrderBy(r => r.TypeVehicleName);

            return await query.ToListAsync();
        }


        public Task<bool> AnyAsync(Expression<Func<Slots, bool>> predicate)
       => _context.Slots.AsNoTracking().AnyAsync(predicate);

    }
}
    

     
