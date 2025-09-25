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
    public class RolBusiness : RepositoryBusiness<Rol, RolDto>, IRolBusiness
    {
        private readonly IRolData _data;
        private readonly IMapper _mapper;
        private readonly ILogger<RolBusiness> _logger;
        public RolBusiness(IRolData data, IMapper mapper, ILogger<RolBusiness> logger)
            : base(data, mapper)
        {
            _data = data;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<RolDto> GetByNameAsync(string name)
        {
            try
            {
                var rolEntity = await _data.GetByNameAsync(name);
                return _mapper.Map<RolDto>(rolEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el rol por nombre");
                throw new BusinessException("Error al obtener el rol por nombre", ex);
            }
        }
        public override async Task<RolDto> Save(RolDto dto)
        {
            try
            {
                dto.Name = dto.Name?.Trim();

                if (string.IsNullOrWhiteSpace(dto.Name))
                    throw new ArgumentException("El campo 'Name' es obligatorio.");
                if (dto.Name!.Length < 2)
                    throw new ArgumentException("El nombre debe tener al menos 2 caracteres.");
                if (dto.Name!.Length > 100)
                    throw new ArgumentException("El nombre no puede superar los 100 caracteres.");

                // Duplicado (case-insensitive)
                var existing = await _data.GetByNameAsync(dto.Name);
                if (existing != null && string.Equals(existing.Name, dto.Name, StringComparison.OrdinalIgnoreCase))
                    throw new ArgumentException($"Ya existe un rol con el nombre '{dto.Name}'.");

                dto.Asset = true; // si manejas activo/deshabilitado

                var entity = _mapper.Map<Rol>(dto);
                entity = await _data.Save(entity);

                return _mapper.Map<RolDto>(entity);
            }
            catch (ArgumentException) { throw; }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar el rol");
                throw new BusinessException("Error al registrar el rol.", ex);
            }
        }

        public override async Task Update(RolDto dto)
        {
            try
            {
                if (dto.Id <= 0)
                    throw new ArgumentException("El Id debe ser mayor que 0.");

                dto.Name = dto.Name?.Trim();

                if (string.IsNullOrWhiteSpace(dto.Name))
                    throw new ArgumentException("El campo 'Name' es obligatorio.");
                if (dto.Name!.Length < 2)
                    throw new ArgumentException("El nombre debe tener al menos 2 caracteres.");
                if (dto.Name!.Length > 100)
                    throw new ArgumentException("El nombre no puede superar los 100 caracteres.");

                // Verificar existencia del registro a actualizar
                var current = await _data.GetById(dto.Id);
                if (current == null)
                    throw new InvalidOperationException($"No existe un rol con Id {dto.Id}.");

                if (current.Asset == false) // si usas soft delete / deshabilitado
                    throw new InvalidOperationException("No se puede actualizar un rol deshabilitado.");

                // Duplicado en otros (case-insensitive)
                var existing = await _data.GetByNameAsync(dto.Name);
                if (existing != null &&
                    existing.Id != dto.Id &&
                    string.Equals(existing.Name, dto.Name, StringComparison.OrdinalIgnoreCase))
                {
                    throw new ArgumentException($"Ya existe otro rol con el nombre '{dto.Name}'.");
                }

                var entity = _mapper.Map<Rol>(dto);
                await _data.Update(entity);
            }
            catch (ArgumentException) { throw; }
            catch (InvalidOperationException) { throw; }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el rol");
                throw new BusinessException("Error al actualizar el rol.", ex);
            }
        }
    }
}
