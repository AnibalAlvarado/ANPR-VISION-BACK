using AutoMapper;
using Business.Interfaces;
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
   
    public class SectorsBusiness : RepositoryBusiness<Sectors, SectorsDto>, ISectorsBusiness
    {
        private readonly ISectorsData _data;
        private readonly IMapper _mapper;
        public SectorsBusiness(ISectorsData data, IMapper mapper)
            : base(data, mapper)
        {
            _data = data;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SectorsDto>> GetAllJoinAsync()
        {
            try
            {
                IEnumerable<SectorsDto> entities = await _data.GetAllJoinAsync();
                if (!entities.Any()) throw new InvalidOperationException("No se encontraron sectores.");
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
                throw new Exception("Error al obtener los sectores.", ex);
            }
        }

        public async Task<IEnumerable<SectorsDto>> GetAllByZoneId(int zoneId)
        {
            try
            {
                if (zoneId < 1) throw new ArgumentException("El id de la zona es invalido.");
                IEnumerable<Sectors> entities = await _data.GetAllByZoneId(zoneId);
                if (!entities.Any()) throw new InvalidOperationException("No se encontraron sectores para la zona seleccionada.");
                return _mapper.Map<IEnumerable<SectorsDto>>(entities);
            }
            catch(InvalidOperationException invEx)
            {
                throw new InvalidOperationException("error: ", invEx);
            }
            catch (ArgumentException argEx)
            {
                throw new ArgumentException("error: ", argEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los sectores de la zona.", ex);
            }
        }
    }
}
