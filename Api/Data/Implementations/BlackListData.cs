using AutoMapper;
using Data.Interfaces;
using Entity.Contexts;
using Entity.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Audit.Services;
using Utilities.Interfaces;

namespace Data.Implementations
{
    public class BlackListData : RepositoryData<BlackList>, IBlackListData
    {
        public BlackListData(ApplicationDbContext context, IConfiguration configuration, IAuditService auditService, ICurrentUserService currentUserService, IMapper mapper)
            : base(context, configuration, auditService, currentUserService, mapper)
        {

        }

        public Task<bool> ExistsAsync<T>(Func<object, bool> value)
        {
            throw new NotImplementedException();
        }
    }
}
