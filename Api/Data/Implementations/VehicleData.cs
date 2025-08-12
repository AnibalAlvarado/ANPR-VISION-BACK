using AutoMapper;
using Dapper;
using Data.Interfaces;
using Entity.Contexts;
using Entity.Dtos;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Utilities.Audit.Services;
using Utilities.Interfaces;

namespace Data.Implementations
{
    public class VehicleData : IVehicleData
    {
        private readonly DbContext _context;
        private readonly IDbConnection _dbConnection;

        public VehicleData(DbContext context, IDbConnection dbConnection)
        {
            _context = context;
            _dbConnection = dbConnection;
        }

        public async Task<Vehicle> GetByIdAsync(int id)
        {
            return await _context.Set<Vehicle>()
                .Include(v => v.TypeVehicle)
                .Include(v => v.Client)
                .Include(v => v.BlackList)
                .Include(v => v.Memberships)
                .Include(v => v.RegisteredVehicles)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            var sql = @"SELECT * FROM Vehicle";
            return await _dbConnection.QueryAsync<Vehicle>(sql);
        }

        public async Task<int> AddAsync(Vehicle entity)
        {
            _context.Set<Vehicle>().Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(Vehicle entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<Vehicle>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<Vehicle>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public Task<Vehicle> GetByPlateAsync(string plate)
        {
            throw new NotImplementedException();
        }

        Task<bool> IVehicleData.UpdateAsync(Vehicle vehicle)
        {
            throw new NotImplementedException();
        }

        Task<bool> IVehicleData.DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
