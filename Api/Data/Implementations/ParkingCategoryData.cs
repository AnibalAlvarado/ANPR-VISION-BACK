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
    public class ParkingCategoryData : RepositoryData<ParkingCategory>, IParkingCategoryData
    {
        public ParkingCategoryData(ApplicationDbContext context, IConfiguration configuration, IAuditService auditService, ICurrentUserService currentUserService, IMapper mapper)
            : base(context, configuration, auditService, currentUserService, mapper)
        {

        }

        public override async Task Update(ParkingCategory entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var local = _context.Set<ParkingCategory>().Local.FirstOrDefault(e => e.Id == entity.Id);

            if (local != null)
            {
                if (!ReferenceEquals(local, entity))
                    _context.Entry(local).CurrentValues.SetValues(entity);
            }
            else
            {
                _context.Set<ParkingCategory>().Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();
        }
    }
}
