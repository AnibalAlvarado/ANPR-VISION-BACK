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
using Utilities.Helpers.Validators;

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
        public override async Task<ZonesDto> Save(ZonesDto dto)
        {
            try
            {
                if (await _data.ExistsAsync(x => x.Name == dto.Name))
                {
                    throw new InvalidOperationException("El nombre de la zona ya se encuentra registrado.");
                }
                Validations.ValidateDto(dto, "Name");

                if (string.IsNullOrWhiteSpace(dto.Name))
                    throw new ArgumentException("El campo Nombre es obligatorio.");
                if (dto.Name.Length < 3)
                    throw new ArgumentException("El nombre debe tener al menos 3 caracteres.");
                if (dto.Name.Length > 100)
                    throw new ArgumentException("El nombre no puede superar los 100 caracteres.");


                dto.Asset = true;

                BaseModel entity = _mapper.Map<Zones>(dto);
                entity = await _data.Save((Zones)entity);

                return _mapper.Map<ZonesDto>(entity);
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
                throw new BusinessException("Error al registrar el tipo de vehículo.", ex);
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
