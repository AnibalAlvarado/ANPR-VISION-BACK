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
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Utilities.Audit.Services;
using Utilities.Interfaces;

namespace Data.Implementations
{
    public class MembershipsData : RepositoryData<Memberships>, IMembershipsData
    {
        public MembershipsData(ApplicationDbContext context, IConfiguration configuration, IAuditService auditService, ICurrentUserService currentUserService, IMapper mapper)
            : base(context, configuration, auditService, currentUserService, mapper)
        {

        }

        public async Task<IEnumerable<MembershipsDto>> GetAllJoinAsync()
        {
            return await _context.Memberships
                .AsNoTracking()
                .Select(p => new MembershipsDto
                {
                    // --- BaseDto ---
                    Id = p.Id,                      // int? en BaseDto
                    Asset = p.Asset,                 // bool? en BaseDto
                    IsDeleted = p.IsDeleted,         // bool en BaseDto

                    // --- GenericDto ---
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    PriceAtPurchase = p.PriceAtPurchase,
                    DurationDays = p.DurationDays,
                    Currency = p.Currency,


                    // --- MembershipType ---
                    MembershipTypeId = p.MembershipTypeId,
                    MembershipType = p.MembershipType != null
                        ? p.MembershipType.Name
                        : null,

                    VehicleId = p.VehicleId,
                    Vehicle = p.Vehicle != null
                        ? p.Vehicle.Plate
                        : null,

                })
                .ToListAsync();
        }


        public Task<bool> ExistsAsync<T>(Func<object, bool> value)
        {
            throw new NotImplementedException();
        }
    }
}
