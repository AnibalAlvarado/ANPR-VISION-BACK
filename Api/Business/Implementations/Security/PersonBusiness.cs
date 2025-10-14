using AutoMapper;
using Business.Interfaces.Security;
using Data.Interfaces.Security;
using Entity.Dtos.Security;
using Entity.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Implementations.Security
{
    public class PersonBusiness : RepositoryBusiness<Person, PersonDto>, IPersonBusiness
    {
        private readonly IPersonData _data;
        public PersonBusiness(IPersonData data, IMapper mapper)
            : base(data, mapper)
        {
            _data = data;
        }
    }
}
