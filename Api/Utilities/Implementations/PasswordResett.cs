using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Contexts;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Utilities.Interfaces;

namespace Utilities.Implementations
{
    public class PasswordResett : IPasswordReset
    {
        private readonly ApplicationDbContext _context;


        public PasswordResett(ApplicationDbContext context)
        {
            _context = context;
        }

   
        public async Task Add(PasswordReset reset)
        {
            _context.PasswordResets.Add(reset);
            await _context.SaveChangesAsync();
        }
   
        public async Task<PasswordReset?> GetValidCode(int userId, string code)
        {
            return await _context.PasswordResets
                .FirstOrDefaultAsync(r =>
                    r.UsuarioId == userId &&
                    r.Code == code &&
                    r.Used == false &&
                    r.ExpiryDate > DateTime.UtcNow);
        }
      

        public async Task MarkAsUsed(PasswordReset reset)
        {
            reset.Used = true;
            _context.PasswordResets.Update(reset);
            await _context.SaveChangesAsync();
        }

      


        public async Task CleanOldResets(int days = 30)
        {
            var old = await _context.PasswordResets
                .Where(r => r.ExpiryDate < DateTime.UtcNow.AddDays(-days))
                .ToListAsync();

            if (old.Any())
            {
                _context.PasswordResets.RemoveRange(old);
                await _context.SaveChangesAsync();
            }
        }


      




    }
}
