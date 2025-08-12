﻿using Entity.Dtos;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IRolBusiness : IRepositoryBusiness<Rol, RolDto>
    {
        Task<RolDto> GetByNameAsync(string name);
    }
}
