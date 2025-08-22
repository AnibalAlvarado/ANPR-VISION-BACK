using AutoMapper;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dtos;
using Entity.DTOs;
using Entity.Model;
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
    public class BlackListBusiness : RepositoryBusiness<BlackList, BlackListDto>, IBlackListBusiness
    {
        private readonly IBlackListData _data;
        private readonly IMapper _mapper;
        public BlackListBusiness(IBlackListData data, IMapper mapper)
            : base(data, mapper)
        {
            _data = data;
            _mapper = mapper;
        }

        public override async Task<BlackListDto> Save(BlackListDto dto)
        {
            try
            {
                Validations.ValidateDto(dto, "Reason", "VehicleId");

                if (dto.VehicleId <= 0)
                    throw new ArgumentException("no se encuentra este Id vehicle.");

                if (!string.IsNullOrWhiteSpace(dto.Reason) && dto.Reason.Length > 250)
                    throw new ArgumentException("El campo Reason no puede tener más de 250 caracteres.");

                if (!string.IsNullOrWhiteSpace(dto.Reason) &&
                    !System.Text.RegularExpressions.Regex.IsMatch(dto.Reason, @"^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\s\.,-]+$"))
                {
                    throw new ArgumentException("El campo Reason contiene caracteres no permitidos.");
                }

                var fechaActual = DateTime.UtcNow;
                if (dto.RestrictionDate != default && dto.RestrictionDate > fechaActual)
                    throw new ArgumentException("La fecha de restricción no puede ser una fecha futura.");

        

                var existe = await _data.ExistsAsync<BlackList>(x =>
                {
                    var entity = x as BlackList;
                    return entity != null && entity.VehicleId == dto.VehicleId && entity.Asset == true;
                });

                if (existe)
                    throw new InvalidOperationException($"El vehículo con ID {dto.VehicleId} ya está en la lista negra.");

                if (dto.Asset == false)
                    throw new ArgumentException("No se puede guardar un registro deshabilitado.");

                dto.RestrictionDate = fechaActual;
                dto.Asset = true;

                BaseModel entity = _mapper.Map<BlackList>(dto);
                entity = await _data.Save((BlackList)entity);

                return _mapper.Map<BlackListDto>(entity);
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
                throw new BusinessException("Error al crear la entidad.", ex);
            }
        }

        public override async Task Update(BlackListDto dto)
        {
            try
            {
                Validations.ValidateDto(dto, "Id", "Reason", "VehicleId");

                if (dto.Id <= 0)
                    throw new ArgumentException("El campo Id debe ser mayor a 0.");


                var entityExistente = await _data.GetById(dto.Id);
                if (entityExistente == null)
                    throw new InvalidOperationException($"No existe un registro con Id {dto.Id}.");

                if (!entityExistente.Asset)
                    throw new InvalidOperationException("No se puede actualizar un registro que está deshabilitado.");

                if (dto.VehicleId <= 0)
                    throw new ArgumentException("El campo VehicleId debe ser mayor a 0.");

                var existeDuplicado = await _data.ExistsAsync<BlackList>(x =>
                {
                    var entity = x as BlackList; 
                    return entity != null && entity.VehicleId == dto.VehicleId && entity.Id != dto.Id && entity.Asset == true;
                });

                if (existeDuplicado)
                    throw new InvalidOperationException($"Ya existe un registro activo para el vehículo con ID {dto.VehicleId}.");

                if (!string.IsNullOrWhiteSpace(dto.Reason) && dto.Reason.Length > 250)
                    throw new ArgumentException("El campo Reason no puede tener más de 250 caracteres.");

                if (!string.IsNullOrWhiteSpace(dto.Reason) &&
                    !System.Text.RegularExpressions.Regex.IsMatch(dto.Reason, @"^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\s\.,-]+$"))
                {
                    throw new ArgumentException("El campo Reason contiene caracteres no permitidos.");
                }

                BaseModel entity = _mapper.Map<BlackList>(dto);
                await _data.Update((BlackList)entity);
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
                throw new BusinessException("Error al actualizar la entidad.", ex);
            }
        }


    }
}
