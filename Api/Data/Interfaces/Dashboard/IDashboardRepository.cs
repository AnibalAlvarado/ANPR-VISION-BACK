using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces.Dashboard
{
    public interface IDashboardRepository
    {
        /// <summary>
        /// Cantidad de vehículos actualmente estacionados (ExitDate == null y no eliminados).
        /// </summary>
        Task<int> GetCurrentParkedCountAsync(CancellationToken ct = default);
    }
}
