using Entity.Dtos;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IRegisteredVehiclesData : IRepositoryData<RegisteredVehicles>
    {
        // Nuevo método para validar si un slot está ocupado
        Task<bool> AnyActiveRegisteredVehicleInSlotAsync(int slotId);

        Task<IEnumerable<RegisteredVehiclesDto>> GetAllJoinAsync();


    }
}
