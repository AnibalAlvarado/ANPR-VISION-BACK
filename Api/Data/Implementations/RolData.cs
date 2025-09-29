using AutoMapper;
using Data.Interfaces;
using Entity.Contexts;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Audit.Services;
using Utilities.Interfaces;

namespace Data.Implementations
{
    public class RolData : RepositoryData<Rol>, IRolData
    {
        public RolData(ApplicationDbContext context, IConfiguration configuration,IAuditService auditService, ICurrentUserService currentUserService, IMapper mapper)
            : base(context, configuration,  auditService, currentUserService, mapper)
        {

        }
        public async Task<Rol?> GetByNameAsync(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name)) return null;
                var normalized = name.Trim().ToUpperInvariant();

                return await _context.Set<Rol>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r =>
                        r.Name != null &&
                        r.Name.ToUpper() == normalized &&
                        !(r.IsDeleted ?? false)
                    );
            }
            catch (Exception ex)
            {
                throw new DataException("Error al obtener el rol por nombre", ex);
            }
        }

        // Si RepositoryData tiene 'virtual Task Update(T entity)' puedes sobreescribir:
        public override async Task Update(Rol entity)
        {
            var dbEntity = await _context.Set<Rol>().FindAsync(entity.Id);
            if (dbEntity == null)
                throw new InvalidOperationException($"No existe el rol con Id {entity.Id}.");

            if (!ReferenceEquals(dbEntity, entity))
            {
                _context.Entry(dbEntity).CurrentValues.SetValues(entity);
            }

            await _context.SaveChangesAsync();
        }
        //public async Task<Rol> GetByNameAsync(string name)
        //{

        //    try
        //    {
        //        return await _context.Set<Rol>()
        //                .FirstOrDefaultAsync(r => r.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new DataException("Error al obtener el rol por nombre", ex);
        //    }
        //}
    }
}
