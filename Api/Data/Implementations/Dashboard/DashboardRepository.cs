using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces.Dashboard;
using Entity.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Data.Implementations.Dashboard
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _db;
        public DashboardRepository(ApplicationDbContext db) => _db = db;

        public Task<int> GetCurrentParkedCountAsync(CancellationToken ct = default)
        {
            return _db.RegisteredVehicles
                .Where(rv => rv.ExitDate == null)     // sigue adentro
                .Where(rv => rv.IsDeleted == null)    
                .CountAsync(ct);
        }
    }
}
