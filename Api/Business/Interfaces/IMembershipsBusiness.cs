using Data.Implementations;
using Entity.Dtos;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IMembershipsBusiness
    {

        Task<IEnumerable<MembershipsDto>> GetAllAsync();
        Task<MembershipsDto> GetByIdAsync(int id);
        Task<MembershipsDto> CreateAsync(MembershipsDto membershipsDto);
        Task<MembershipsDto> UpdateAsync(int id, MembershipsDto membershipsDto);

        Task<bool> DeleteAsync(int id);
    }
}
