using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces;
using Entity.Models;

namespace Data.Interfaces
{
    public interface IPersonParkingData : IRepositoryData<PersonParking>
    {
        Task<IEnumerable<PersonParking>> GetAllJoinAsync();
        Task<IEnumerable<Parking>> GetParkingsByPersonIdAsync(int personId);

    }
}
