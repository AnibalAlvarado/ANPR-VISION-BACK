using Entity.Dtos;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface ISlotsData : IRepositoryData<Slots>
    {
       Task<IEnumerable<SlotsDto>> GetAllJoinAsync();
        Task<IEnumerable<Slots>> GetAllBySectorId(int sectorId);
        Task<bool> ExistsAsync<T>(Func<object, bool> value);
        Task GetByIdAsync<T>(int sectorsId);

        Task<bool> AnyAsync(Expression<Func<Slots, bool>> predicate); // 👈
        Task<int> CountExistingBySectorAsync(int sectorId);
    }
}
