using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface ISlotsData : IRepositoryData<Slots>
    {

        public Task<IEnumerable<Slots>> GetAvailableSlotsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Slots> GetSlotByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Slots>> GetSlotsBySectorIdAsync(int sectorId)
        {
            throw new NotImplementedException();
        }

        public Task<Slots> GetSlotWithRegisteredVehicleAsync(int slotId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateSlotAvailabilityAsync(int slotId, bool isAvailable)
        {
            throw new NotImplementedException();
        }
    }
}
