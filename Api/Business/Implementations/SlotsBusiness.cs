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

    public class SlotsBusiness : RepositoryBusiness<Slots, SlotsDto>, ISlotsBusiness
    {
        private readonly ISlotsData _data;
        private readonly IMapper _mapper;
        public SlotsBusiness(ISlotsData data, IMapper mapper)
            : base(data, mapper)
        {
            _data = data;
            _mapper = mapper;
        }


        public async Task<IEnumerable<SlotsDto>> GetAllJoinAsync()
        {
            try
            {
                IEnumerable<SlotsDto> entities = await _data.GetAllJoinAsync();
                if (!entities.Any()) throw new InvalidOperationException("No se encontraron slots.");
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
                throw new Exception("Error al obtener los slots.", ex);
            }
        }

        public async Task<IEnumerable<SlotsDto>> GetAllBySectorId(int sectorId)
        {
            try
            {
                if (sectorId < 1) throw new ArgumentException("El id del sector es invalido.");
                IEnumerable<Slots> entities = await _data.GetAllBySectorId(sectorId);
                if (!entities.Any()) throw new InvalidOperationException("No se encontraron slots para el sector seleccionado.");
                return _mapper.Map<IEnumerable<SlotsDto>>(entities);
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
                throw new Exception("Error al obtener los slots del sector.", ex);
            }
        }

    }
}
