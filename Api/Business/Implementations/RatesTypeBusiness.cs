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

    public class RatesTypeBusiness : RepositoryBusiness<RatesType, RatesTypeDto>, IRatesTypeBusiness
    {
        private readonly IRatesTypeData _data;
        private readonly IMapper _mapper;
        public RatesTypeBusiness(IRatesTypeData data, IMapper mapper)
            : base(data, mapper)
        {
            _data = data;
            _mapper = mapper;
        }

        public override async Task<RatesTypeDto> Save(RatesTypeDto dto)
        {
            try
            {
                Validations.ValidateDto(dto, "Name");

                dto.Name = dto.Name?.Trim();
                if (string.IsNullOrWhiteSpace(dto.Name))
                    throw new ArgumentException("El nombre del tipo de tarifa es obligatorio.");
                if (dto.Name.Length > 50)
                    throw new ArgumentException("El nombre del tipo de tarifa no puede superar los 50 caracteres.");

                // 🔍 Validar duplicado por nombre (case-insensitive)
                var exists = await _data.ExistsAsync(rt =>
                    rt.Name.ToLower() == dto.Name.ToLower() &&
                    rt.Asset // si manejas habilitado/soft-delete
                );
                if (exists)
                    throw new ArgumentException($"Ya existe un tipo de tarifa con el nombre '{dto.Name}'.");

                dto.Asset = true;

                var entity = _mapper.Map<RatesType>(dto);
                entity = await _data.Save(entity);

                return _mapper.Map<RatesTypeDto>(entity);
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
                throw new BusinessException("Error al crear el registro.", ex);
            }
        }


        public override async Task Update(RatesTypeDto dto)
        {
            try
            {
                // Incluye Id en la validación de dto si tu helper lo requiere
                Validations.ValidateDto(dto, "Id", "Name");

                if (dto.Id <= 0)
                    throw new ArgumentException("Debe seleccionar un tipo de tarifa válido.");

                var current = await _data.GetById(dto.Id)
                             ?? throw new InvalidOperationException("El tipo de tarifa seleccionado no existe.");

                dto.Name = dto.Name?.Trim();
                if (string.IsNullOrWhiteSpace(dto.Name))
                    throw new ArgumentException("El nombre del tipo de tarifa es obligatorio.");
                if (dto.Name.Length > 50)
                    throw new ArgumentException("El nombre del tipo de tarifa no puede superar los 50 caracteres.");

                // 🔍 Duplicado por nombre en otros registros (case-insensitive)
                var existsOther = await _data.ExistsAsync(rt =>
                    rt.Name.ToLower() == dto.Name.ToLower() &&
                    rt.Id != dto.Id &&
                    rt.Asset
                );
                if (existsOther)
                    throw new ArgumentException($"Ya existe otro tipo de tarifa con el nombre '{dto.Name}'.");

                var entity = _mapper.Map<RatesType>(dto);
                await _data.Update(entity);
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
                throw new BusinessException("Error al actualizar el registro.", ex);
            }
        }

    }
}
