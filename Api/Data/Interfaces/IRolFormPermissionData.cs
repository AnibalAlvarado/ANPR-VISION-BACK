﻿using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IRolFormPermissionData : IRepositoryData<RolFormPermission>
    {
        public Task<IEnumerable<RolFormPermission>> GetAllJoinAsync();

    }
}
