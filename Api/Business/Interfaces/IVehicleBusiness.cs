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
    public interface IVehicleBusiness
    {
        Task<IEnumerable<VehicleDto>> GetAllAsync();
        Task<VehicleDto> GetByIdAsync(int id);
        Task<VehicleDto> GetByPlateAsync(string plate);
        Task<int> AddAsync(VehicleDto vehicleDto);
        Task<bool> UpdateAsync(int id, VehicleDto vehicleDto);
        Task<bool> DeleteAsync(int id);
    }
}
