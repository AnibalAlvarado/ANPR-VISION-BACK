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

namespace Business.Implementations
{
    public class PermissionBusiness : RepositoryBusiness<Permission, PermissionDto>, IPermissionBusiness
    {
        private readonly IPermissionData _data;
        private readonly IMapper _mapper;
        public PermissionBusiness(IPermissionData data, IMapper mapper)
            : base(data, mapper)
        {
            _data = data;
            _mapper = mapper;
        }
        public override async Task<PermissionDto> Save(PermissionDto dto)
        {
            try
            {
                // (Opcional) helper de validaciones si lo usas en tu proyecto
                // Validations.ValidateDto(dto, "Name");

                dto.Name = dto.Name?.Trim();

                if (string.IsNullOrWhiteSpace(dto.Name))
                    throw new ArgumentException("El campo 'Name' es obligatorio.");
                if (dto.Name.Length < 2)
                    throw new ArgumentException("El nombre debe tener al menos 2 caracteres.");
                if (dto.Name.Length > 100)
                    throw new ArgumentException("El nombre no puede superar los 100 caracteres.");

                //  Duplicado por nombre (case-insensitive)
                var exists = await _data.ExistsAsync(p =>
                    p.Name.ToLower() == dto.Name.ToLower() &&
                    p.Asset // si manejas habilitado/soft-delete
                );
                if (exists)
                    throw new ArgumentException($"Ya existe un permiso con el nombre '{dto.Name}'.");

                dto.Asset = true;

                var entity = _mapper.Map<Permission>(dto);
                entity = await _data.Save(entity);

                return _mapper.Map<PermissionDto>(entity);
            }
            catch (ArgumentException) { throw; }
            catch (Exception ex)
            {
                throw new BusinessException("Error al crear el permiso.", ex);
            }
        }

        public override async Task Update(PermissionDto dto)
        {
            try
            {
                // Validations.ValidateDto(dto, "Id", "Name");

                if (dto.Id <= 0)
                    throw new ArgumentException("El Id debe ser mayor que 0.");

                dto.Name = dto.Name?.Trim();

                var current = await _data.GetById(dto.Id);
                if (current == null)
                    throw new InvalidOperationException($"No existe un permiso con Id {dto.Id}.");

                if (string.IsNullOrWhiteSpace(dto.Name))
                    throw new ArgumentException("El campo 'Name' es obligatorio.");
                if (dto.Name.Length < 2)
                    throw new ArgumentException("El nombre debe tener al menos 2 caracteres.");
                if (dto.Name.Length > 100)
                    throw new ArgumentException("El nombre no puede superar los 100 caracteres.");

                if (!current.Asset)
                    throw new InvalidOperationException("No se puede actualizar un permiso deshabilitado.");

                //  Duplicado en otros (case-insensitive)
                var existsOther = await _data.ExistsAsync(p =>
                    p.Name.ToLower() == dto.Name.ToLower() &&
                    p.Id != dto.Id &&
                    p.Asset
                );
                if (existsOther)
                    throw new ArgumentException($"Ya existe otro permiso con el nombre '{dto.Name}'.");

                var entity = _mapper.Map<Permission>(dto);
                await _data.Update(entity);
            }
            catch (ArgumentException) { throw; }
            catch (InvalidOperationException) { throw; }
            catch (Exception ex)
            {
                throw new BusinessException("Error al actualizar el permiso.", ex);
            }
        }
    }
}
