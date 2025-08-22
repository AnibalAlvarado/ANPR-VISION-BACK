using AutoMapper;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dtos;
using Entity.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Implementations
{
  
    public class ParkingBusiness : RepositoryBusiness<Parking, ParkingDto>, IParkingBusiness
    {
        private readonly IParkingData _data;
        private readonly IMapper _mapper;
        public ParkingBusiness(IParkingData data, IMapper mapper)
            : base(data, mapper)
        {
            _data = data;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ParkingDto>> GetAllJoinAsync()
        {
            try
            {
                IEnumerable<ParkingDto> entities = await _data.GetAllJoinAsync();
                if (!entities.Any()) throw new InvalidOperationException("No se encontraron los parqueaderos.");
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
                throw new Exception("Error al obtener los parqueaderos.", ex);
            }
        }
    
    }
}
