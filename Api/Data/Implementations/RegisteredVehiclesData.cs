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
    public class RegisteredVehicleData : RepositoryData<RegisteredVehicles>, IRegisteredVehiclesData
    {
        public RegisteredVehicleData(ApplicationDbContext context, IConfiguration configuration, IAuditService auditService, ICurrentUserService currentUserService, IMapper mapper)
            : base(context, configuration, auditService, currentUserService, mapper)
        {

        }

        // Método que valida si un slot está ocupado
        public async Task<bool> AnyActiveRegisteredVehicleInSlotAsync(int slotId)
        {
            return await _context.RegisteredVehicles
                .AnyAsync(rv => rv.SlotsId == slotId && rv.ExitDate == null);
        }

        public async Task<IEnumerable<RegisteredVehiclesDto>> GetAllJoinAsync()
        {
            return await _context.RegisteredVehicles
                .AsNoTracking()
                .Select(p => new RegisteredVehiclesDto
                {
                    // --- BaseDto ---
                    Id = p.Id,                      // int? en BaseDto
                    Asset = p.Asset,                 // bool? en BaseDto
                    IsDeleted = p.IsDeleted,         // bool en BaseDto

                    EntryDate = p.EntryDate,
                    ExitDate = p.ExitDate,


                    // --- ZonesDto ---
                    VehicleId = p.VehicleId,
                    Vehicle = p.Vehicle != null
                        ? p.Vehicle.Plate
                        : null,

                    //slots

                    SlotsId = p.SlotsId,
                    Slots = p.Slots != null

                    ? p.Slots.Name
                    :null

                })
                .ToListAsync();
        }
    }
}
