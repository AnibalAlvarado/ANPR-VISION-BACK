using AutoMapper;
using Data.Interfaces;
using Entity.Contexts;
using Entity.Dtos;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Audit.Services;
using Utilities.Helpers;
using Utilities.Interfaces;

namespace Data.Implementations
{
    public class ClientData : RepositoryData<Client>, IClientData
    {
        private readonly ILogger<ClientData> _logger;
        private readonly IAuditService _auditService;
        public ClientData(ApplicationDbContext context, IConfiguration configuration, IAuditService auditService, ICurrentUserService currentUserService, IMapper mapper, ILogger<ClientData> logger)
            : base(context, configuration, auditService, currentUserService, mapper)
        {
            _logger = logger;
            _auditService = auditService;

        }

        public async Task<IEnumerable<ClientDto>> GetAllJoinAsync()
        {
            return await _context.Clients
                .AsNoTracking()
                .Select(p => new ClientDto
                {
                    // --- BaseDto ---
                    Id = p.Id,                      // int? en BaseDto
                    Asset = p.Asset,                 // bool? en BaseDto
                    IsDeleted = p.IsDeleted,         // bool en BaseDto

                    // --- GenericDto ---
                    Name = p.Name,                   // string en GenericDto

                    // --- ZonesDto ---
                    PersonaId = p.PersonId,
                    Person = p.Person != null
                        ? p.Person.FirstName
                        : null
                })
                .ToListAsync();
        }

    }
}
