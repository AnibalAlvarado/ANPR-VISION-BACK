using Entity.Models;
using Entity.Models.Audit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Audit.Services
{
    public interface IAuditService
    {
        void LogInsert(Vehicle entity, string v, object userId);
        Task SaveAuditAsync(AuditLog entry);
    }
}
