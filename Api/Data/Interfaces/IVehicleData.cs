using Entity.Dtos;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IVehicleData
    {
        Task<IEnumerable<Vehicle>> GetAllAsync();
        Task<Vehicle> GetByIdAsync(int id);
        Task<Vehicle> GetByPlateAsync(string plate);
        Task<int> AddAsync(Vehicle vehicle);
        Task<bool> UpdateAsync(Vehicle vehicle);
        Task<bool> DeleteAsync(int id);
    }
}
