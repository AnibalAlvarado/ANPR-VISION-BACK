using System;
using System.Collections.Generic;
using System.Linq;
using Entity.Models;
using System.Text;
using System.Threading.Tasks;
using Entity.Dtos;

namespace Data.Interfaces
{

    public interface ICamaraData : IRepositoryData<Camera>
    {
        Task<IEnumerable<CameraDto>> GetAllJoinAsync();
        Task<bool> ExistsDuplicateAsync(CameraDto dto);
    }
}
