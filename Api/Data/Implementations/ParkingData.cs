using AutoMapper;
using Data.Interfaces;
using Entity.Contexts;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Utilities.Audit.Services;
using Utilities.Interfaces;

namespace Data.Implementations
{
    public class ParkingData : RepositoryData<Parking>, IParkingData
    {
        private readonly ILogger<ParkingData> _logger;

        public ParkingData(ApplicationDbContext context, IConfiguration configuration, IAuditService auditService, ICurrentUserService currentUserService, IMapper mapper, ILogger<ParkingData> logger)
            : base(context, configuration, auditService, currentUserService, mapper)
        {
            _logger = logger;

        }

       

    }
}
