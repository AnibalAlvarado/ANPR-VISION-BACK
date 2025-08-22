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
using Utilities.Exceptions;
using Utilities.Helpers.Validators;

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
        public override async Task<SlotsDto> Save(SlotsDto dto)
        {
            try
            {
                Validations.ValidateDto(dto, "IsAvailable", "SectorsId");

                if (dto.SectorsId <= 0)
                    throw new ArgumentException("El campo SectorsId debe ser mayor a 0.");

                // Obtener el sector correctamente usando un repositorio adecuado
                // Suponiendo que _data solo maneja Slots, necesitas un repositorio de sectores.
                // Aquí se asume que tienes acceso a un ISectorsData o similar.
                // Si no lo tienes, deberías inyectarlo en el constructor.
                var sector = await _data.GetById(dto.SectorsId);
                if (sector == null)
                    throw new InvalidOperationException($"El sector con Id {dto.SectorsId} no existe.");

                var slotDuplicado = await _data.ExistsAsync<Slots>(
                    x => ((Slots)x).SectorsId == dto.SectorsId &&
                         ((Slots)x).Id == dto.Id &&
                         ((Slots)x).Asset == true
                );
                if (slotDuplicado)
                    throw new InvalidOperationException(
                        "Ya existe un slot activo en el mismo sector con el mismo Id."
                    );

                dto.Asset = true;

                BaseModel entity = _mapper.Map<Slots>(dto);
                entity = await _data.Save((Slots)entity);

                return _mapper.Map<SlotsDto>(entity);
            }
            catch (InvalidOperationException invOe)
            {
                throw new InvalidOperationException($"Error: {invOe.Message}", invOe);
            }
            catch (ArgumentException argEx)
            {
                throw new ArgumentException($"Error: {argEx.Message}");
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al crear el registro del slot.", ex);
            }
        }

    }
}
