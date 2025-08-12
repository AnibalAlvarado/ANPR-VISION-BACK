using Entity.Dtos;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
     public interface IClientData :IRepositoryData<Client>
    {

        public Task<bool> ExistsByPersonIdAsync(int personId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Client>> GetAllWithPersonAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Client> GetByIdWithPersonAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Client>> GetByPersonIdAsync(int personId)
        {
            throw new NotImplementedException();
        }
    }
}
