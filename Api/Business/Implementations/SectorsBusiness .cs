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

        // Pseudocódigo detallado:
        // 1. El método Save intenta validar que no exista un sector duplicado usando _data.ExistsAsync<Sectors>.
        // 2. Sin embargo, ISectorsData no tiene un método ExistsAsync definido.
        // 3. Solución: Reemplazar la llamada a ExistsAsync por una consulta manual usando GetAllByZoneId y filtrando por TypeVehicleId y Asset.
        // 4. Obtener todos los sectores de la zona, filtrar por TypeVehicleId y Asset == true, y verificar si existe alguno.

        public override async Task<SectorsDto> Save(SectorsDto dto)
        {
            try
            {
            
                Validations.ValidateDto(dto, "Capacity", "ZonesId", "TypeVehicleId");

           
                if (dto.Capacity <= 0)
                    throw new ArgumentException("El campo Capacity debe ser mayor a 0.");
                if (dto.Capacity > 5000)
                    throw new ArgumentException("El campo Capacity no puede superar los 5000 espacios.");

               
                if (dto.ZonesId <= 0)
                    throw new ArgumentException("El campo ZonesId debe ser mayor a 0.");

             
                if (dto.TypeVehicleId <= 0)
                    throw new ArgumentException("El campo TypeVehicleId debe ser mayor a 0.");


                var sectoresZona = await _data.GetAllByZoneId(dto.ZonesId);
                var sectorDuplicado = sectoresZona.Any(x => x.TypeVehicleId == dto.TypeVehicleId && x.Asset == true);
                if (sectorDuplicado)
                    throw new InvalidOperationException(
                        "Ya existe un sector activo en la misma zona para el mismo tipo de vehículo."
                    );

                dto.Asset = true;

                BaseModel entity = _mapper.Map<Sectors>(dto);
                entity = await _data.Save((Sectors)entity);

                return _mapper.Map<SectorsDto>(entity);
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
                throw new BusinessException("Error al crear el registro del sector.", ex);
            }
        }

        public override async Task Update(SectorsDto dto)
        {
            try
            {
                Validations.ValidateDto(dto, "Id", "Capacity", "ZonesId", "TypeVehicleId");

                if (dto.Id <= 0)
                    throw new ArgumentException("El campo Id debe ser mayor a 0.");

                var sectorExistente = await _data.GetById(dto.Id);
                if (sectorExistente == null)
                    throw new InvalidOperationException($"No existe un sector con Id {dto.Id}.");

                if (!sectorExistente.Asset)
                    throw new InvalidOperationException("No se puede actualizar un sector deshabilitado.");

                if (dto.Capacity <= 0)
                    throw new ArgumentException("El campo Capacity debe ser mayor a 0.");
                if (dto.Capacity > 5000)
                    throw new ArgumentException("El campo Capacity no puede superar los 5000 espacios.");

                if (dto.ZonesId <= 0)
                    throw new ArgumentException("El campo ZonesId debe ser mayor a 0.");

                if (dto.TypeVehicleId <= 0)
                    throw new ArgumentException("El campo TypeVehicleId debe ser mayor a 0.");

                var sectoresZona = await _data.GetAllByZoneId(dto.ZonesId);
                var sectorDuplicado = sectoresZona.Any(x =>
                    x.TypeVehicleId == dto.TypeVehicleId &&
                    x.Id != dto.Id &&
                    x.Asset == true
                );
                if (sectorDuplicado)
                    throw new InvalidOperationException(
                        "Ya existe otro sector activo en la misma zona para el mismo tipo de vehículo."
                    );

                BaseModel entity = _mapper.Map<Sectors>(dto);
                await _data.Update((Sectors)entity);
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
                throw new BusinessException("Error al actualizar el registro del sector.", ex);
            }
        }


    }
}
