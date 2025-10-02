using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Dtos;
using Entity.Models;

namespace Business.Interfaces
{
    public interface IPersonParkingBusiness: IRepositoryBusiness<PersonParking, PersonParkingDto>
    {
        Task<IEnumerable<PersonParkingDto>> GetAllJoinAsync();
        Task<IEnumerable<ParkingDto>> GetParkingsByPersonIdAsync(int personId);


    }
}
