using Data.Interfaces;
using Entity.Contexts;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Data.Implementations
{
    public class MembershipsData : IMembershipsData
    {
        private readonly ApplicationDbContext _context;
        private readonly IDbConnection _dbConnection;

        public MembershipsData(ApplicationDbContext context, IDbConnection dbConnection)
        {
            _context = context;
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Memberships>> GetAllAsync()
        {
            return await _context.Memberships
                .Include(m => m.MemberShipType)
                .Include(m => m.Vehicles)
                .ToListAsync();
        }

        public async Task<Memberships> GetByIdAsync(int id)
        {
            return await _context.Memberships
                .Include(m => m.MemberShipType)
                .Include(m => m.Vehicles)
                .FirstOrDefaultAsync(m => m.id == id);
        }

        public async Task<Memberships> CreateAsync(Memberships memberships)
        {
            _context.Memberships.Add(memberships);
            await _context.SaveChangesAsync();
            return memberships;
        }

        public async Task<Memberships> UpdateAsync(Memberships memberships)
        {
            _context.Entry(memberships).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return memberships;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Memberships.FindAsync(id);
            if (entity == null) return false;

            _context.Memberships.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
