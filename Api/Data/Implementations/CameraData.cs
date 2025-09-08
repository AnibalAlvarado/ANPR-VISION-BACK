using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Data.Interfaces;
using Entity.Contexts;
using Entity.Dtos;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Utilities.Audit.Services;
using Utilities.Interfaces;

namespace Data.Implementations
{
    public class CameraData : RepositoryData<Camera>, ICamaraData
    {
        private readonly ILogger<CameraData> _logger;

        public CameraData(ApplicationDbContext context, IConfiguration configuration, IAuditService auditService, ICurrentUserService currentUserService, IMapper mapper, ILogger<CameraData> logger)
            : base(context, configuration, auditService, currentUserService, mapper)
        {
            _logger = logger;


        }

        public async Task<IEnumerable<CameraDto>> GetAllJoinAsync()
        {
            return await _context.Cameras
                .AsNoTracking()
                .Select(p => new CameraDto
                {
                    // --- BaseDto ---
                    Id = p.Id,                      // int? en BaseDto
                    Asset = p.Asset,                 // bool? en BaseDto
                    IsDeleted = p.IsDeleted,         // bool en BaseDto

                    // --- GenericDto ---

                    Name = p.Name,
                    Resolution = p.Resolution,
                    Url = p.Url,

                    // --- ZonesDto ---
                    ParkingId = p.ParkingId,
                    Parking = p.Parking != null
                        ? p.Parking.Name
                        : null
                })
                .ToListAsync();
        }

        public async Task<bool> ExistsDuplicateAsync(CameraDto dto)
        {
            return await _context.Cameras
                .AsNoTracking()
                .AnyAsync(x =>
                    x.Resolution.Trim().ToLower() == dto.Resolution.Trim().ToLower() &&
                    x.Url.Trim().ToLower() == dto.Url.Trim().ToLower() &&
                    x.ParkingId == dto.ParkingId &&
                    x.Id != dto.Id &&
                    x.Asset == true
                );
        }




    }
}
