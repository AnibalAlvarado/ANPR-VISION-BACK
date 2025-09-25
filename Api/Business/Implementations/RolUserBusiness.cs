using AutoMapper;
using Business.Interfaces;
using Data.Implementations;
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
    public class RolUserBusiness : RepositoryBusiness<RolUser, RolUserDto>, IRolUserBusiness
    {
        private readonly IRolUserData _data;
        private readonly IMapper _mapper;
        private readonly ILogger<RolUserBusiness> _logger;

        public RolUserBusiness(IRolUserData data, IMapper mapper, ILogger<RolUserBusiness> logger)
            : base(data, mapper)
        {
            _data = data;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<bool> ExistsAsync(int userId, int roleId)
        {
            try
            {
                return await _data.ExistsAsync(userId, roleId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar existencia del rol para el usuario");
                throw new BusinessException("Error al verificar existencia del rol para el usuario", ex);
            }
        }
        public async Task<IEnumerable<RolUserDto>> GetAllJoinAsync()
        {
            var entities = await _data.GetAllJoinAsync();
            return _mapper.Map<IEnumerable<RolUserDto>>(entities);
        }

        public override async Task<RolUserDto> GetById(int id)
        {
            var entity = await _data.GetById(id);
            if (entity == null)
                throw new Exception($"No se encontró RolUser con ID {id}");
            return _mapper.Map<RolUserDto>(entity);
        }

        public override async Task<RolUserDto> Save(RolUserDto dto)
        {
            try
            {
                // Validaciones mínimas de IDs
                if (dto == null) throw new ArgumentException("Datos inválidos.");
                if (dto.UserId <= 0) throw new ArgumentException("UserId inválido.");
                if (dto.RolId <= 0) throw new ArgumentException("RolId inválido.");

                // Evitar duplicados: mismo UserId + RolId y que no esté marcado como eliminado
                // Requiere que tu repo tenga ExistsAsync(Expression<Func<RolUser,bool>>)
                var exists = await _data.ExistsAsync(r =>
                    r.UserId == dto.UserId &&
                    r.RolId == dto.RolId &&
                    (r.IsDeleted != true)    // null-safe: considera null como no eliminado
                );
                if (exists)
                    throw new ArgumentException("El usuario ya tiene asignado ese rol.");

                // Valores por defecto para nuevo registro
                dto.Asset = true;
                dto.IsDeleted = false;

                var entity = _mapper.Map<RolUser>(dto);
                var saved = await _data.Save(entity);

                return _mapper.Map<RolUserDto>(saved);
            }
            catch (ArgumentException) { throw; }
            catch (Exception ex)
            {
                throw new BusinessException("Error al registrar RolUser.", ex);
            }
        }

        public override async Task Update(RolUserDto dto)
        {
            try
            {
                if (dto == null) throw new ArgumentException("Datos inválidos.");
                if (dto.Id <= 0) throw new ArgumentException("Id inválido.");
                if (dto.UserId <= 0) throw new ArgumentException("UserId inválido.");
                if (dto.RolId <= 0) throw new ArgumentException("RolId inválido.");

                // Verificar que exista el registro que quiero actualizar
                var current = await _data.GetById(dto.Id);
                if (current == null)
                    throw new InvalidOperationException($"No existe el registro con Id {dto.Id}.");

                // Verificar duplicado en otro registro (excluir el propio Id)
                var existsOther = await _data.ExistsAsync(r =>
                    r.Id != dto.Id &&
                    r.UserId == dto.UserId &&
                    r.RolId == dto.RolId &&
                    (r.IsDeleted != true)
                );
                if (existsOther)
                    throw new ArgumentException("Ya existe otro registro con esa misma combinación usuario-rol.");

                // Mapear únicamente los campos que quieres actualizar (evita sobrescribir relaciones)
                current.UserId = dto.UserId;
                current.RolId = dto.RolId;

                // Si manejas Asset / IsDeleted desde DTO, actualízalos; si no, coméntalos
                current.IsDeleted = dto.IsDeleted;

                await _data.Update(current);
            }
            catch (ArgumentException) { throw; }
            catch (InvalidOperationException) { throw; }
            catch (Exception ex)
            {
                throw new BusinessException("Error al actualizar RolUser.", ex);
            }
        }


        public override async Task<int> Delete(int id)
        {
            return await _data.Delete(id);
        }

        public override async Task<bool> PermanentDelete(int id)
        {
            return await _data.PermanentDelete(id);
        }
    }
}
