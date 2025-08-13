using AutoMapper;
using Data.Interfaces;
using Entity.Contexts;
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
    public class ZonesData : RepositoryData<Zones>, IZonesData
    {
        private readonly ILogger<ZonesData> _logger;

        public ZonesData(ApplicationDbContext context, IConfiguration configuration, IAuditService auditService, ICurrentUserService currentUserService, IMapper mapper, ILogger<ZonesData> logger)
            : base(context, configuration, auditService, currentUserService, mapper)
        {
            _logger = logger;


        }

        public async Task<IEnumerable<Zones>> GetAllJoinAsync()
        {
            await AuditAsync("GetAllJoinAsync");

            return await _context.Zones
                .Include(x => x.Parking)
                .ToListAsync();
        }


    }
}
