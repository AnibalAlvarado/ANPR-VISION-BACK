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

     
        Task<IEnumerable<VehicleDto>> GetAllJoinAsync();
        // Nuevo método para registrar vehículo + slot automáticamente
        Task<RegisteredVehicles> RegisterVehicleWithSlotAsync(int vehicleId);

        Task<RegisteredVehiclesDto?> GetActiveVehicleBySlotAsync(int slotId);


    }
}
