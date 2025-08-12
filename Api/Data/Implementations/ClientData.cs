using AutoMapper;
using Data.Interfaces;
using Entity.Contexts;
using Entity.Model;
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
using Utilities.Interfaces;

namespace Data.Implementations
{
    public class ClientData
    {
        private readonly DbContext _context;
        private readonly ILogger<ClientData> _logger;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public ClientData(DbContext context, ILogger<ClientData> logger, IMapper mapper, IConfiguration config)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _config = config;
        }
    }
}
