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
 
    public class TypeVehicleBusiness : RepositoryBusiness<TypeVehicle, TypeVehicleDto>, ITypeVehicleBusiness
    {
        private readonly ITypeVehicleData  _data;
        private readonly IMapper _mapper;
        public TypeVehicleBusiness(ITypeVehicleData data, IMapper mapper)
            : base(data, mapper)
        {
            _data = data;
            _mapper = mapper;
        }
        public override async Task<TypeVehicleDto> Save(TypeVehicleDto dto)
        {
            try
            {
                Validations.ValidateDto(dto, "Name");

                if (string.IsNullOrWhiteSpace(dto.Name))
                    throw new ArgumentException("El campo Nombre es obligatorio.");
                if (dto.Name.Length < 3)
                    throw new ArgumentException("El nombre debe tener al menos 3 caracteres.");
                if (dto.Name.Length > 100)
                    throw new ArgumentException("El nombre no puede superar los 100 caracteres.");

              
                dto.Asset = true;

                BaseModel entity = _mapper.Map<TypeVehicle>(dto);
                entity = await _data.Save((TypeVehicle)entity);

                return _mapper.Map<TypeVehicleDto>(entity);
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
        public override async Task Update(TypeVehicleDto dto)
        {
            try
            {
                // 🔹 Validar campos obligatorios
                Validations.ValidateDto(dto, "Id", "Name");

                // 🔹 Validar Id
                if (dto.Id <= 0)
                    throw new ArgumentException("El campo Id debe ser mayor que 0.");

                // 🔹 Verificar que el tipo de vehículo exista
                var tipoExistente = await _data.GetById(dto.Id);
                if (tipoExistente == null)
                    throw new InvalidOperationException($"No existe un tipo de vehículo con Id {dto.Id}.");

                // 🔹 No permitir actualizar registros deshabilitados
                if (!tipoExistente.Asset)
                    throw new InvalidOperationException("No se puede actualizar un tipo de vehículo deshabilitado.");

                // 🔹 Validar nombre
                if (string.IsNullOrWhiteSpace(dto.Name))
                    throw new ArgumentException("El campo Nombre es obligatorio.");
                if (dto.Name.Length < 3)
                    throw new ArgumentException("El nombre debe tener al menos 3 caracteres.");
                if (dto.Name.Length > 100)
                    throw new ArgumentException("El nombre no puede superar los 100 caracteres.");

              
            
                BaseModel entity = _mapper.Map<TypeVehicle>(dto);
                await _data.Update((TypeVehicle)entity);
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
                throw new BusinessException("Error al actualizar el tipo de vehículo.", ex);
            }
        }

    }
}
