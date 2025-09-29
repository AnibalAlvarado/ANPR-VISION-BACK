using AutoMapper;
using Data.Interfaces;
using Entity.Contexts;
using Entity.Dtos;
using Entity.Dtos.vehicle;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Utilities.Audit.Services;
using Utilities.Interfaces;

namespace Data.Implementations
{
    public class VehicleData : RepositoryData<Vehicle>, IVehicleData
    {
        private readonly ILogger<VehicleData> _logger;

        public VehicleData(ApplicationDbContext context, IConfiguration configuration, IAuditService auditService, ICurrentUserService currentUserService, IMapper mapper, ILogger<VehicleData> logger)
            : base(context, configuration, auditService, currentUserService, mapper)
        {
            _logger = logger;
        }
        public async Task<IEnumerable<VehicleDto>> GetAllJoinAsync()
        {
            return await _context.Vehicles
                .AsNoTracking()
                .Select(p => new VehicleDto
                {
                    // --- BaseDto ---
                    Id = p.Id,                      // int? en BaseDto
                    Asset = p.Asset,                 // bool? en BaseDto
                    IsDeleted = p.IsDeleted,         // bool en BaseDto

                    // --- GenericDto ---
                    Plate = p.Plate,                   // string en GenericDto
                    Color = p.Color,

                    // --- ZonesDto ---
                    TypeVehicleId = p.TypeVehicleId,
                    TypeVehicle = p.TypeVehicle != null
                        ? p.TypeVehicle.Name
                        : null,

                    ClientId = p.ClientId,
                    Client = p.Client != null
                        ? p.Client.Name
                        : null
                })
                .ToListAsync();
        }
        public async Task<RegisteredVehicles?> GetActiveRegisteredVehicleBySlotAsync(int slotId)
        {
            return await _context.RegisteredVehicles
                .Include(rv => rv.Vehicle) // Para traer también la info del vehículo
                .FirstOrDefaultAsync(rv => rv.SlotsId == slotId && rv.ExitDate == null);
        }

        public async Task<IEnumerable<VehicleClientListDto>> GetByClientIdWithPresenceAsync(int clientId)
        {
            // Subconjunto de RV activos (ExitDate NULL)
            var activeRvs = _context.RegisteredVehicles
                .AsNoTracking()
                .Where(rv => rv.ExitDate == null);

            // LEFT JOIN (group join) a un único RV activo (si existiera) para traer slot/fechas
            var query =
                from v in _context.Vehicles.AsNoTracking()
                where v.ClientId == clientId && v.IsDeleted != true
                join a in activeRvs on v.Id equals a.VehicleId into g
                from a in g
                    .OrderByDescending(x => x.Id)   // si hay varios activos anómalos, tomamos el último
                    .Take(1)
                    .DefaultIfEmpty()
                orderby v.Id descending
                select new VehicleClientListDto
                {
                    // --- VehicleDto base ---
                    Id = v.Id,
                    Asset = v.Asset,
                    IsDeleted = v.IsDeleted,
                    Plate = v.Plate,
                    Color = v.Color,
                    TypeVehicleId = v.TypeVehicleId,
                    TypeVehicle = v.TypeVehicle != null ? v.TypeVehicle.Name : null,
                    ClientId = v.ClientId,
                    Client = v.Client != null ? v.Client.Name : null,

                    // --- Estado ---
                    IsInside = a != null,
                    ActiveRegisteredId = a != null ? a.Id : (int?)null,
                    ActiveSlotId = a != null ? a.SlotsId : (int?)null,
                    ActiveSlotName = a != null ? a.Slots != null ? a.Slots.Name : null : null,
                    ActiveEntryDate = a != null ? a.EntryDate : (DateTime?)null
                };

            return await query.ToListAsync();
        }





    }


}
