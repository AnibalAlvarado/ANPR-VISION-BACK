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

    }
}
