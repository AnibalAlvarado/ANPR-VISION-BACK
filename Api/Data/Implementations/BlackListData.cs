using AutoMapper;
using Dapper;
using Data.Interfaces;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Implementations
{
    public class BlackListData : IBlackListData
    {
        private readonly DbContext _context;
        private readonly IDbConnection _dbConnection;

        public BlackListData(DbContext context, IDbConnection dbConnection)
        {
            _context = context;
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<BlackList>> GetAllAsync()
        {
            var sql = @"SELECT * FROM BlackList";
            return await _dbConnection.QueryAsync<BlackList>(sql);
        }

        public async Task<BlackList> GetByIdAsync(int id)
        {
            return await _context.Set<BlackList>()
                .Include(b => b.Vehicle)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<BlackList> CreateAsync(BlackList entity)
        {
            _context.Set<BlackList>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<BlackList> UpdateAsync(BlackList entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Set<BlackList>().FindAsync(id);
            if (entity == null)
                return false;

            _context.Set<BlackList>().Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
