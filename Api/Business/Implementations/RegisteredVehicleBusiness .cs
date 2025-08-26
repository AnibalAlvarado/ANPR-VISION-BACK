using AutoMapper;
using Business.Interfaces;
using Data.Implementations;
using Data.Interfaces;
using Entity.Dtos;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Implementations
{
   
    public class RegisteredVehicleBusiness : RepositoryBusiness<RegisteredVehicles, RegisteredVehiclesDto>, IRegisteredVehicleBusiness
    {
        private readonly IRegisteredVehiclesData _data;
        public RegisteredVehicleBusiness(IRegisteredVehiclesData data, IMapper mapper)
            : base(data, mapper)
        {
            _data = data;
        }

        public async Task<IEnumerable<RegisteredVehiclesDto>> GetAllJoinAsync()
        {
            try
            {
                IEnumerable<RegisteredVehiclesDto> entities = await _data.GetAllJoinAsync();
                if (!entities.Any()) throw new InvalidOperationException("No se encontraron registros de vehiculos.");
                return entities;
            }
            catch (InvalidOperationException invEx)
            {
                throw new InvalidOperationException("error: ", invEx);
            }
            catch (ArgumentException argEx)
            {
                throw new ArgumentException("error: ", argEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las registros .", ex);
            }
        }

    }
}
