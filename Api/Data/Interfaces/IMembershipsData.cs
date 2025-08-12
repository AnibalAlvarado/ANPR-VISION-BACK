using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IMembershipsData 
    {
        Task<IEnumerable<Memberships>> GetAllAsync();
        Task<Memberships> GetByIdAsync(int id);
        Task<Memberships> CreateAsync(Memberships memberships);
        Task<Memberships> UpdateAsync(Memberships memberships);
        Task<bool> DeleteAsync(int id);
    }
}
