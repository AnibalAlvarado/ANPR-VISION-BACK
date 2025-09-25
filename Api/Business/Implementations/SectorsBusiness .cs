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
                Validations.ValidateDto(dto, "Name", "Capacity", "ZonesId");

                dto.Name = dto.Name?.Trim();

                if (string.IsNullOrWhiteSpace(dto.Name))
                    throw new ArgumentException("El campo Name es obligatorio.");
                if (dto.Name.Length < 2)
                    throw new ArgumentException("El nombre debe tener al menos 2 caracteres.");
                if (dto.Name.Length > 100)
                    throw new ArgumentException("El nombre no puede superar los 100 caracteres.");

                if (dto.Capacity <= 0)
                    throw new ArgumentException("El campo Capacity debe ser mayor a 0.");
                if (dto.Capacity > 5000)
                    throw new ArgumentException("El campo Capacity no puede superar los 5000 espacios.");

                if (dto.ZonesId <= 0)
                    throw new ArgumentException("El campo ZonesId debe ser mayor a 0.");

                // 🔍 Duplicado por NOMBRE en la misma zona (ignora mayúsculas)
                var sectoresMismaZona = await _data.GetAllByZoneId(dto.ZonesId);
                var nombreDuplicado = sectoresMismaZona.Any(s =>
                    s.Name != null &&
                    s.Name.Trim().ToLower() == dto.Name!.ToLower() &&
                    s.Asset // si usas soft delete / habilitado
                );
                if (nombreDuplicado)
                    throw new ArgumentException($"Ya existe un sector con el nombre '{dto.Name}' en esta zona.");

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

        //  UPDATE con validación de duplicado (excluye el propio Id)
        public override async Task Update(SectorsDto dto)
        {
            try
            {
                Validations.ValidateDto(dto, "Id", "Name", "Capacity", "ZonesId");

                if (dto.Id <= 0)
                    throw new ArgumentException("El campo Id debe ser mayor a 0.");

                dto.Name = dto.Name?.Trim();

                Sectors sectorExistente = await _data.GetById(dto.Id);
                if (sectorExistente == null)
                    throw new InvalidOperationException($"No existe un sector con Id {dto.Id}.");

                if (string.IsNullOrWhiteSpace(dto.Name))
                    throw new ArgumentException("El campo Name es obligatorio.");
                if (dto.Name.Length < 2)
                    throw new ArgumentException("El nombre debe tener al menos 2 caracteres.");
                if (dto.Name.Length > 100)
                    throw new ArgumentException("El nombre no puede superar los 100 caracteres.");

                if (dto.Capacity <= 0)
                    throw new ArgumentException("El campo Capacity debe ser mayor a 0.");
                if (dto.Capacity > 5000)
                    throw new ArgumentException("El campo Capacity no puede superar los 5000 espacios.");

                if (dto.ZonesId <= 0)
                    throw new ArgumentException("El campo ZonesId debe ser mayor a 0.");

                if (!sectorExistente.Asset)
                    throw new InvalidOperationException("No se puede actualizar un sector deshabilitado.");

                //  Duplicado por NOMBRE en la misma zona, excluyendo este mismo Id
                var sectoresMismaZona = await _data.GetAllByZoneId(dto.ZonesId);
                var nombreDuplicadoOtro = sectoresMismaZona.Any(s =>
                    s.Name != null &&
                    s.Name.Trim().ToLower() == dto.Name!.ToLower() &&
                    s.Id != dto.Id &&
                    s.Asset
                );
                if (nombreDuplicadoOtro)
                    throw new ArgumentException($"Ya existe otro sector con el nombre '{dto.Name}' en esta zona.");

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
