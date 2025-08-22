using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IMembershipsData : IRepositoryData<Memberships>
    {
        Task<bool> ExistsAsync<T>(Func<object, bool> value);
    }
}
