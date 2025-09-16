using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces.Dashboard
{
    public interface IDashboardBusiness
    {
        Task<int> GetCurrentParkedCountAsync(CancellationToken ct = default);
    }
}
