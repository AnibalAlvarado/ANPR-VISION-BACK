using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces.Dashboard;
using Data.Interfaces.Dashboard;

namespace Business.Implementations.Dashboard
{
    public class DashboardBusiness : IDashboardBusiness
    {
        private readonly IDashboardRepository _repo;
        public DashboardBusiness(IDashboardRepository repo) => _repo = repo;

        public Task<int> GetCurrentParkedCountAsync(CancellationToken ct = default)
            => _repo.GetCurrentParkedCountAsync(ct);
    }
}
