using AutoMapper;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dtos;
using Entity.DtoSpecific.RolFormPermission;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Exceptions;

namespace Business.Implementations
{
    public class RolFormPermissionBusiness : RepositoryBusiness<RolFormPermission, RolFormPermissionDto>, IRolFormPermissionBusiness
    {
        private readonly IRolFormPermissionData _data;
        private readonly IMapper _mapper;

        public RolFormPermissionBusiness(IRolFormPermissionData data, IMapper mapper)
            : base(data, mapper)
        {
            _data = data;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RolFormPermissionDto>> GetAllJoinAsync()
        {
            var entities = await _data.GetAllJoinAsync();
            return _mapper.Map<IEnumerable<RolFormPermissionDto>>(entities);
        }

        public async Task<RolFormPermissionGroupedDto?> GetAllByRolId(int rolId)
        {
            // Traer los datos planos desde Data
            RolFormPermissionGroupedDto? groupedData = await _data.GetAllByRolId(rolId);

            if (groupedData == null)
                return null;

            return groupedData;
        }

        public override async Task<RolFormPermissionDto> Save(RolFormPermissionDto dto)
        {
            try
            {

                bool exists = await _data.ExistsAsync(
                      x => x.FormId == dto.FormId && x.RolId == dto.RolId && x.PermissionId == dto.PermissionId
                  );

                if (exists)
                    throw new InvalidOperationException(
                        $"ya se encuentra Existente este registro."
                    );
                var entity = _mapper.Map<RolFormPermission>(dto);
                entity = await _data.Save(entity);

                return _mapper.Map<RolFormPermissionDto>(entity);
            }
            catch (InvalidOperationException invOp)
            {
                throw new InvalidOperationException($"Error: {invOp.Message}", invOp);
            }
            catch (ArgumentException argEx)
            {
                throw new ArgumentException($"Error: {argEx.Message}");
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al registrar el formulario.", ex);
            }
        }
    }
}
