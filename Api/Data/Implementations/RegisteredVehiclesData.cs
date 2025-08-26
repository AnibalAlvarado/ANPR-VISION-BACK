using AutoMapper;
using Data.Interfaces;
using Entity.Contexts;
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
    }
}
