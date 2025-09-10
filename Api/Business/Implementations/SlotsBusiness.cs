using AutoMapper;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dtos;
using Entity.Dtos.Dashboard;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Exceptions;
using Utilities.Helpers.Validators;

namespace Business.Implementations
{

    public class SlotsBusiness : RepositoryBusiness<Slots, SlotsDto>, ISlotsBusiness
    {
        private readonly ISlotsData _data;
        private readonly IMapper _mapper;
        private readonly IRepositoryData<Sectors> _sectors;
        public SlotsBusiness(ISlotsData data, IMapper mapper, IRepositoryData<Sectors> sectors)
            : base(data, mapper)
        {
            _data = data;
            _mapper = mapper;
            _sectors = sectors;
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
        public override async Task<SlotsDto> Save(SlotsDto dto)
        {
            try
            {
                Validations.ValidateDto(dto, "IsAvailable", "SectorsId");
                if (dto.SectorsId <= 0)
                    throw new ArgumentException("El campo SectorsId debe ser mayor a 0.");

                // ✅ Validar existencia en la tabla Sectors
                var sector = await _sectors.GetById(dto.SectorsId);
                if (sector == null)
                    throw new InvalidOperationException($"El sector con Id {dto.SectorsId} no existe.");

                // ✅ Dedupe razonable (ver notas abajo)
                var existeDuplicado = await _data.AnyAsync(
                    s => s.SectorsId == dto.SectorsId
                         && s.Name == dto.Name    // o la clave única que definas
                         && s.Asset == true
                         && s.IsDeleted == false
                         && (dto.Id == 0 || s.Id != dto.Id) // excluye el propio al editar
                );
                if (existeDuplicado)
                    throw new InvalidOperationException("Ya existe un slot activo con ese nombre en el mismo sector.");

                dto.Asset = true;
                var entity = _mapper.Map<Slots>(dto);
                entity = await _data.Save(entity);
                return _mapper.Map<SlotsDto>(entity);
            }
            catch (InvalidOperationException invOe) { throw new InvalidOperationException($"Error: {invOe.Message}", invOe); }
            catch (ArgumentException argEx) { throw new ArgumentException($"Error: {argEx.Message}"); }
            catch (Exception ex) { throw new BusinessException("Error al crear el registro del slot.", ex); }
        }
        public async Task<List<SlotsAvailabilityByTypeDto>> GetAvailabilityByParkingGroupedByTypeAsync(int parkingId)
        {
            if (parkingId < 1) throw new ArgumentException("El id del parqueadero es inválido.");
            var data = await _data.GetAvailabilityByParkingGroupedByTypeAsync(parkingId);
            return data;
        }

    }
}
