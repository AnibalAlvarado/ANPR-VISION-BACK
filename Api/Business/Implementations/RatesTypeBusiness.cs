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
                Validations.ValidateDto(dto,"Name");
                if (dto.Name.Length > 50)
                    throw new ArgumentException("El nombre del tipo de tarifa no puede contener mas de 70 caracteres.");

                dto.Asset = true;

                BaseModel entity = _mapper.Map<RatesType>(dto);
                entity = await _data.Save((RatesType)entity);

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
                Validations.ValidateDto(dto, "Name");
                if (dto.Id <= 0)
                    throw new ArgumentException("No ha seleccioando ningun tipo de tarifa.");
                RatesType ratesTypeExistente = await _data.GetById(dto.Id) ?? throw new InvalidOperationException($"Seleccone un tipo de tarifa válida.");
                if (dto.Name.Length > 50)
                    throw new ArgumentException("El nombre del tipo de tarifa no puede contener mas de 70 caracteres.");

                BaseModel entity = _mapper.Map<RatesType>(dto);
                await _data.Update((RatesType)entity);
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
