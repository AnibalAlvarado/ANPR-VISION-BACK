using AutoMapper;
using Data.Interfaces;
using Entity.Contexts;
using Entity.Dtos;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Utilities.Audit.Services;
using Utilities.Interfaces;

namespace Data.Implementations
{
    public class ParkingData : RepositoryData<Parking>, IParkingData
    {
        public ParkingData(ApplicationDbContext context, IConfiguration configuration, IAuditService auditService, ICurrentUserService currentUserService, IMapper mapper)
            : base(context, configuration, auditService, currentUserService, mapper)
        {

        }

        public async Task<IEnumerable<ParkingDto>> GetAllJoinAsync()
        {
            return await _context.Parkings
                .AsNoTracking()
                .Select(p => new ParkingDto
                {
                    // --- BaseDto ---
                    Id = p.Id,                      // int? en BaseDto
                    Asset = p.Asset,                 // bool? en BaseDto
                    IsDeleted = p.IsDeleted,         // bool en BaseDto

                    // --- GenericDto ---
                    Name = p.Name,                   // string en GenericDto

                    // --- ParkingDto ---
                    Location = p.Location,
                    ParkingCategoryId = p.ParkingCategoryId,
                    ParkingCategory = p.ParkingCategory != null
                        ? p.ParkingCategory.Name
                        : null
                })
                .ToListAsync();
        }


    }
}
