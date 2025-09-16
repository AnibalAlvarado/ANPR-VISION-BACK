using Entity.Dtos;
using Entity.Dtos.Dashboard;
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
        Task<bool> AnyActiveRegisteredVehicleInSlotAsync(int slotId);

        Task<IEnumerable<RegisteredVehiclesDto>> GetAllJoinAsync();

        Task<int> GetTotalCurrentlyParkedByParkingAsync(int parkingId);
        Task<int> GetTotalCurrentlyParkedAsync();
        Task<VehicleTypeDistributionDto> GetVehicleTypeDistributionGlobalAsync(bool includeZeros = true);
        Task<List<OccupancyItemDto>> GetSectorOccupancyByZoneAsync(int zoneId);
    }
}
