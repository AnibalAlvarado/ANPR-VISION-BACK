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
    public class FormModuleBusiness : RepositoryBusiness<FormModule, FormModuleDto>, IFormModuleBusiness
    {
        private readonly IFormModuleData _data;
        private readonly IMapper _mapper;
        public FormModuleBusiness(IFormModuleData data, IMapper mapper)
            : base(data, mapper)
        {
            _data = data;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FormModuleDto>> GetAllJoinAsync()
        {
            var entities = await _data.GetAllJoinAsync();
            return _mapper.Map<IEnumerable<FormModuleDto>>(entities);
        }
        public override async Task<FormModuleDto> Save(FormModuleDto dto)
        {
            try
            {

                bool exists = await _data.ExistsAsync(
                      x => x.FormId == dto.FormId && x.ModuleId == dto.ModuleId
                  );

                if (exists)
                    throw new InvalidOperationException(
                        $"ya se encuentra Existente este registro."
                    );
                var entity = _mapper.Map<FormModule>(dto);
                entity = await _data.Save(entity);

                return _mapper.Map<FormModuleDto>(entity);
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
