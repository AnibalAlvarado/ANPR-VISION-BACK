using Entity.Dtos;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IVehicleBusiness : IRepositoryBusiness<Vehicle, VehicleDto>
    {
        public Task<int> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<VehicleDto>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<List<ExpandoObject>> GetAllDynamicAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<VehicleDto>> GetAllPaginatedAsync(QueryParameters query, Expression<Func<Vehicle, bool>>? filter = null, Func<IQueryable<Vehicle>, IQueryable<Vehicle>>? include = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<VehicleDto> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> PermanentDelete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<VehicleDto> Save(VehicleDto entityDto)
        {
            throw new NotImplementedException();
        }

        public Task Update(VehicleDto entityDto)
        {
            throw new NotImplementedException();
        }
    }
}
