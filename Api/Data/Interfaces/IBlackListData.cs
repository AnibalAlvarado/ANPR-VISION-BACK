using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public  interface IBlackListData 
    {
        Task<IEnumerable<BlackList>> GetAllAsync();
        Task<BlackList> GetByIdAsync(int id);
        Task<BlackList> CreateAsync(BlackList entity);
        Task<BlackList> UpdateAsync(BlackList entity);
        Task<bool> DeleteAsync(int id);
    }
}
