using AutoMapper;
using Data.Interfaces.Parameter;
using Entity.Contexts;
using Entity.Dtos.Parameter;
using Entity.Models.Parameter;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;
using Utilities.Audit.Services;
using Utilities.Interfaces;

namespace Data.Implementations.Parameter
{
    public class RatesData : RepositoryData<Rates>, IRatesData
    {
        public RatesData(ApplicationDbContext context, IConfiguration configuration, IAuditService auditService, ICurrentUserService currentUserService, IMapper mapper)
            : base(context, configuration, auditService, currentUserService, mapper)
        {

        }

        public async Task<IEnumerable<RatesDto>> GetAllJoinAsync()
        {
            return await _context.Rates
                .AsNoTracking()
                .Select(p => new RatesDto
                {
                    // --- BaseDto ---
                    Id = p.Id,                      
                    Asset = p.Asset,                 
                    IsDeleted = p.IsDeleted,


                    Name = p.Name,
                    Type = p.Type,
                    Amount = p.Amount,
                    StarHour = p.StarHour,
                    EndHour = p.EndHour,
                    Year = p.Year,

                    // --- ParkingDto ---
                    ParkingId = p.ParkingId,
                    Parking = p.Parking != null
                        ? p.Parking.Name
                        : null,

                        //RatesTypeDto

                    RatesTypeId = p.RatesTypeId,
                    RatesType = p.RatesType != null
                        ? p.RatesType.Name
                        : null,

                    //TypeVehicleDto

                    TypeVehicleId = p.TypeVehicleId,
                    TypeVehicle = p.TypeVehicle != null
                        ? p.TypeVehicle.Name
                        : null

                })
                .ToListAsync();
        }

        public async Task<IEnumerable<RatesDto>> GetByParkingAsync(int parkingId)
        {
            return await _context.Rates
                .AsNoTracking()
                .Where(r => r.ParkingId == parkingId && (r.IsDeleted == false || r.IsDeleted == null))
                .Select(p => new RatesDto
                {
                    Id = p.Id,
                    Asset = p.Asset,
                    IsDeleted = p.IsDeleted,

                    Name = p.Name,
                    Type = p.Type,
                    Amount = p.Amount,
                    StarHour = p.StarHour,
                    EndHour = p.EndHour,
                    Year = p.Year,

                    ParkingId = p.ParkingId,
                    Parking = p.Parking != null ? p.Parking.Name : null,

                    RatesTypeId = p.RatesTypeId,
                    RatesType = p.RatesType != null ? p.RatesType.Name : null,

                    TypeVehicleId = p.TypeVehicleId,
                    TypeVehicle = p.TypeVehicle != null ? p.TypeVehicle.Name : null
                })
                .ToListAsync();
        }




    }
}
