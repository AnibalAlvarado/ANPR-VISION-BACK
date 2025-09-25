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
    public class FormBusiness : RepositoryBusiness<Form, FormDto>, IFormBusiness
    {
        private readonly IFormData _data;
        private readonly IMapper _mapper;
        public FormBusiness(IFormData data, IMapper mapper)
            : base(data, mapper)
        {
            _mapper = mapper;
            _data = data;
        }


        public override async Task<FormDto> Save(FormDto dto)
        {
            try
            {
                // ✅ Validaciones base
                Validations.ValidateDto(dto, "Name");

                dto.Name = dto.Name?.Trim();

                if (string.IsNullOrWhiteSpace(dto.Name))
                    throw new ArgumentException("El campo 'Name' es obligatorio.");
                if (dto.Name.Length < 3)
                    throw new ArgumentException("El nombre debe tener al menos 3 caracteres.");
                if (dto.Name.Length > 100)
                    throw new ArgumentException("El nombre no puede superar los 100 caracteres.");

                // ✅ No duplicar por Name (case-insensitive)
                var existsByName = await _data.ExistsAsync(f =>
                    f.Name.ToLower() == dto.Name!.ToLower()
                );
                if (existsByName)
                    throw new ArgumentException($"Ya existe un formulario con el nombre '{dto.Name}'.");

                dto.Asset = true; // si manejas habilitado/activo

                var entity = _mapper.Map<Form>(dto);
                entity = await _data.Save(entity);

                return _mapper.Map<FormDto>(entity);
            }
            catch (ArgumentException) { throw; }
            catch (Exception ex)
            {
                throw new BusinessException("Error al registrar el formulario.", ex);
            }
        }

        public override async Task Update(FormDto dto)
        {
            try
            {
                // ✅ Validaciones base
                Validations.ValidateDto(dto, "Id", "Name");

                if (dto.Id <= 0)
                    throw new ArgumentException("El Id debe ser mayor que 0.");

                dto.Name = dto.Name?.Trim();

                if (string.IsNullOrWhiteSpace(dto.Name))
                    throw new ArgumentException("El campo 'Name' es obligatorio.");
                if (dto.Name.Length < 3)
                    throw new ArgumentException("El nombre debe tener al menos 3 caracteres.");
                if (dto.Name.Length > 100)
                    throw new ArgumentException("El nombre no puede superar los 100 caracteres.");

                // ✅ Verificar existencia del registro
                var existing = await _data.GetById(dto.Id);
                if (existing == null)
                    throw new InvalidOperationException($"No existe un formulario con Id {dto.Id}.");

                if (!existing.Asset) // quita esto si no manejas soft delete
                    throw new InvalidOperationException("No se puede actualizar un formulario deshabilitado.");

                // ✅ No duplicar por Name en otros registros (case-insensitive)
                var existsOtherByName = await _data.ExistsAsync(f =>
                    f.Name.ToLower() == dto.Name!.ToLower() &&
                    f.Id != dto.Id
                );
                if (existsOtherByName)
                    throw new ArgumentException($"Ya existe otro formulario con el nombre '{dto.Name}'.");

                var entity = _mapper.Map<Form>(dto);
                await _data.Update(entity);
            }
            catch (ArgumentException) { throw; }
            catch (InvalidOperationException) { throw; }
            catch (Exception ex)
            {
                throw new BusinessException("Error al actualizar el formulario.", ex);
            }
        }
    }
}
