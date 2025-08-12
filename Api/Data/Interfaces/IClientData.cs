using Entity.Dtos;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IClientData
    {
        Task<Client> CreateAsync(Client entity);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsByPersonIdAsync(int personId);

        Task<IEnumerable<Client>> GetAllAsync();
        Task<IEnumerable<Client>> GetAllWithPersonAsync();

        Task<Client> GetByIdAsync(int id);
        Task<Client> GetByIdWithPersonAsync(int id);

        Task<IEnumerable<Client>> GetByPersonIdAsync(int personId);

        Task<Client> UpdateAsync(int id, Client entity);
    }
}
