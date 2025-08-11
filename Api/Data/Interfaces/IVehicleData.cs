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
    public interface IVehicleData: IRepositoryData<Vehicle>
    {
        public Task<Vehicle> GetVehicleByPlateAsync(string plate)
        {
            throw new NotImplementedException();
        }



        public Task<IEnumerable<Vehicle>> GetAllVehiclesWithDetailsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Vehicle>> GetRegisteredVehiclesAsync()
        {
            throw new NotImplementedException();
        }

    
        public Task<IEnumerable<Vehicle>> GetVehiclesByClientIdAsync(int clientId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Vehicle>> GetVehiclesInBlackListAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Vehicle>> GetVehiclesWithMembershipsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Vehicle> GetVehicleWithDetailsByIdAsync(int id)
        {
            throw new NotImplementedException();
        }


       




        public Task<PagedResult<TDto>> GetAllPaginatedAsync<TDto>(QueryParameters query, Expression<Func<Client, bool>>? filter = null, Func<IQueryable<Client>, IQueryable<Client>>? include = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

       

        public Task<Client> Save(Client entity)
        {
            throw new NotImplementedException();
        }

        public Task Update(Client entity)
        {
            throw new NotImplementedException();
        }


    }
}
