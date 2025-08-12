using Entity.Dtos;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IClientBusiness : IRepositoryBusiness<Client, ClientDto>
    {

        public Task<ClientDto> CreateAsync(ClientDto clientDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ClientDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ClientDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ClientDto> UpdateAsync(int id, ClientDto clientDto)
        {
            throw new NotImplementedException();
        }
    }
}
