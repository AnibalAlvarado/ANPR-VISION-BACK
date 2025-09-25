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
    public class ModuleBusiness : RepositoryBusiness<Module, ModuleDto>, IModuleBusiness
    {
        private readonly IModuleData _data;
        private readonly IMapper _mapper;
        public ModuleBusiness(IModuleData data, IMapper mapper)
            : base(data, mapper)
        {
            _data = data;
            _mapper = mapper;
        }

        public override async Task<ModuleDto> Save(ModuleDto dto)
        {
            try
            {
                Validations.ValidateDto(dto, "Name");

                dto.Name = dto.Name?.Trim();

                if (string.IsNullOrWhiteSpace(dto.Name))
                    throw new ArgumentException("El nombre es obligatorio.");
                if (dto.Name.Length < 2)
                    throw new ArgumentException("El nombre debe tener al menos 2 caracteres.");
                if (dto.Name.Length > 100)
                    throw new ArgumentException("El nombre no puede superar los 100 caracteres.");

                //  Duplicado por nombre (case-insensitive)
                var exists = await _data.ExistsAsync(m =>
                    m.Name.ToLower() == dto.Name.ToLower() &&
                    m.Asset // quita este filtro si tu entidad no maneja 'Asset'
                );
                if (exists)
                    throw new ArgumentException($"Ya existe un módulo con el nombre '{dto.Name}'.");

                dto.Asset = true; // si manejas habilitado/soft-delete

                var entity = _mapper.Map<Module>(dto);
                entity = await _data.Save(entity);

                return _mapper.Map<ModuleDto>(entity);
            }
            catch (ArgumentException) { throw; }
            catch (Exception ex)
            {
                throw new BusinessException("Error al crear el módulo.", ex);
            }
        }

        public override async Task Update(ModuleDto dto)
        {
            try
            {
                Validations.ValidateDto(dto, "Id", "Name");

                if (dto.Id <= 0)
                    throw new ArgumentException("El Id debe ser mayor que 0.");

                var current = await _data.GetById(dto.Id);
                if (current == null)
                    throw new InvalidOperationException($"No existe un módulo con Id {dto.Id}.");
                if (!current.Asset) // quita si no manejas 'Asset'
                    throw new InvalidOperationException("No se puede actualizar un módulo deshabilitado.");

                dto.Name = dto.Name?.Trim();

                if (string.IsNullOrWhiteSpace(dto.Name))
                    throw new ArgumentException("El nombre es obligatorio.");
                if (dto.Name.Length < 2)
                    throw new ArgumentException("El nombre debe tener al menos 2 caracteres.");
                if (dto.Name.Length > 100)
                    throw new ArgumentException("El nombre no puede superar los 100 caracteres.");

                //  Duplicado en otros registros (case-insensitive)
                var existsOther = await _data.ExistsAsync(m =>
                    m.Name.ToLower() == dto.Name.ToLower() &&
                    m.Id != dto.Id &&
                    m.Asset // quita si no manejas 'Asset'
                );
                if (existsOther)
                    throw new ArgumentException($"Ya existe otro módulo con el nombre '{dto.Name}'.");

                var entity = _mapper.Map<Module>(dto);
                await _data.Update(entity);
            }
            catch (ArgumentException) { throw; }
            catch (InvalidOperationException) { throw; }
            catch (Exception ex)
            {
                throw new BusinessException("Error al actualizar el módulo.", ex);
            }
        }
    }
}
