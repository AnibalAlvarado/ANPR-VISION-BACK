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
                if (await _data.ExistsAsync(x => x.Name == dto.Name))
                {
                    throw new InvalidOperationException("El nombre del rol ya se encuentra registrado.");
                }
                Validations.ValidateDto(dto, "Name");
                if (dto.Name.Length > 50)
                    throw new ArgumentException("El nombre del tipo de tarifa no puede contener mas de 70 caracteres.");

                dto.Asset = true;

                BaseModel entity = _mapper.Map<Rol>(dto);
                entity = await _data.Save((Rol)entity);

                return _mapper.Map<RolDto>(entity);
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
    }
}
