using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface ITypeVehicleData : IRepositoryData<TypeVehicle>
    {
        public Task<IEnumerable<TypeVehicle>> GetAllWithRelationsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TypeVehicle> GetByIdWithRelationsAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
