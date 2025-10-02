using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces;
using Entity.Contexts;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Utilities.Audit.Services;
using Utilities.Interfaces;

namespace Data.Implementations
{
    public class PersonParkingData : RepositoryData<PersonParking>, IPersonParkingData
    {
        public PersonParkingData(ApplicationDbContext context, IConfiguration configuration, IAuditService auditService, ICurrentUserService currentUserService, AutoMapper.IMapper mapper)
            : base(context, configuration, auditService, currentUserService, mapper)
        {
        }
        public async Task<IEnumerable<PersonParking>> GetAllJoinAsync()
        {
            return await _context.PersonParkings
                .Include(x => x.Person)
                .Include(x => x.Parking)
                .ToListAsync();
        }

        //obtener parqueaderos asociados a persona
        public async Task<IEnumerable<Parking>> GetParkingsByPersonIdAsync(int personId)
        {
            return await _context.PersonParkings
                .Where(pp => pp.PersonId == personId && (pp.IsDeleted == null || pp.IsDeleted == false))
                .Include(pp => pp.Parking)
                .Select(pp => pp.Parking)
                .ToListAsync();
        }
    }
}
