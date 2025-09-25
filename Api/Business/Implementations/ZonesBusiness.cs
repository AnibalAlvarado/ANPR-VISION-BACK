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
using Utilities.Exceptions;

namespace Business.Implementations
{
 
    public class ZonesBusiness : RepositoryBusiness<Zones,ZonesDto>, IZonesBusiness
    {
        private readonly IZonesData _data;
        private readonly IMapper _mapper;
        public ZonesBusiness(IZonesData data, IMapper mapper)
            : base(data, mapper)
        {
            _data = data;
            _mapper = mapper;
        }

        public override async Task<ZonesDto> Save(ZonesDto dto)
        {
            try
            {
                // Normalización
                dto.Name = dto.Name?.Trim();
                // dto.Code = dto.Code?.Trim(); // si tienes código

                // Reglas básicas
                if (string.IsNullOrWhiteSpace(dto.Name))
                    throw new ArgumentException("El campo 'Name' es obligatorio.");
                if (dto.ParkingId <= 0)
                    throw new ArgumentException("El 'ParkingId' es obligatorio y debe ser mayor que 0.");
                if (dto.Name.Length < 2)
                    throw new ArgumentException("El nombre debe tener al menos 2 caracteres.");
                if (dto.Name.Length > 100)
                    throw new ArgumentException("El nombre no puede superar los 100 caracteres.");

                // Duplicado: misma zona (Name) en el mismo parqueadero (ParkingId)
                var exists = await _data.ExistsAsync(z =>
                    z.ParkingId == dto.ParkingId &&
                    z.Name.ToLower() == dto.Name.ToLower()
                );
                if (exists)
                    throw new ArgumentException($"Ya existe una zona con el nombre '{dto.Name}' en este parqueadero.");

                dto.Asset = true;

                var entity = _mapper.Map<Zones>(dto);
                entity = await _data.Save(entity);

                return _mapper.Map<ZonesDto>(entity);
            }
            catch (ArgumentException) { throw; }
            catch (Exception ex)
            {
                // Usa tu excepción de negocio si la tienes
                throw new BusinessException("Error al registrar la zona.", ex);
            }
        }

        // ✅ UPDATE con validación de duplicado (excluye el propio Id)
        public override async Task Update(ZonesDto dto)
        {
            try
            {
                if (dto.Id <= 0)
                    throw new ArgumentException("El Id debe ser mayor que 0.");

                dto.Name = dto.Name?.Trim();
                // dto.Code = dto.Code?.Trim(); // si aplica

                if (string.IsNullOrWhiteSpace(dto.Name))
                    throw new ArgumentException("El campo 'Name' es obligatorio.");
                if (dto.ParkingId <= 0)
                    throw new ArgumentException("El 'ParkingId' es obligatorio y debe ser mayor que 0.");
                if (dto.Name.Length < 2)
                    throw new ArgumentException("El nombre debe tener al menos 2 caracteres.");
                if (dto.Name.Length > 100)
                    throw new ArgumentException("El nombre no puede superar los 100 caracteres.");

                // Verificar que exista
                var current = await _data.GetById(dto.Id);
                if (current == null)
                    throw new InvalidOperationException($"No existe una zona con Id {dto.Id}.");

                if (!current.Asset)
                    throw new InvalidOperationException("No se puede actualizar una zona deshabilitada.");

                // Duplicado en otros registros del mismo parking
                var existsOther = await _data.ExistsAsync(z =>
                    z.ParkingId == dto.ParkingId &&
                    z.Name.ToLower() == dto.Name.ToLower() &&
                    z.Id != dto.Id
                );
                if (existsOther)
                    throw new ArgumentException($"Ya existe otra zona con el nombre '{dto.Name}' en este parqueadero.");

                var entity = _mapper.Map<Zones>(dto);
                await _data.Update(entity);
            }
            catch (ArgumentException) { throw; }
            catch (InvalidOperationException) { throw; }
            catch (Exception ex)
            {
                throw new BusinessException("Error al actualizar la zona.", ex);
            }
        }

        public async Task<IEnumerable<ZonesDto>> GetAllJoinAsync()
        {
            try
            {
                IEnumerable<ZonesDto> entities = await _data.GetAllJoinAsync();
                if (!entities.Any()) throw new InvalidOperationException("No se encontraron zonas.");
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
                throw new Exception("Error al obtener las zonas .", ex);
        }
        }

        public async Task<IEnumerable<ZonesDto>> GetAllByParkingId(int parkingId)
        {
            try
            {
                if (parkingId < 1) throw new ArgumentException("El id del estacionamiento es inv·lido.");
                IEnumerable<Zones> entities = await _data.GetAllByParkingId(parkingId);
                if (!entities.Any()) throw new InvalidOperationException("No se encontraron zonas para el estacionamiento.");
                return _mapper.Map<IEnumerable<ZonesDto>>(entities);
            }
            catch (InvalidOperationException invEx)
            {
                throw new InvalidOperationException("error: ", invEx);
            }
            catch (ArgumentException argEx)
            {
                throw new ArgumentException("error: ",argEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las zonas del estacionamiento.", ex);
            }
        }
    }
}
