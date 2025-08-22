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
 
    public class ParkingCategoryBusiness : RepositoryBusiness<ParkingCategory, ParkingCategoryDto>, IParkingCategoryBusiness
    {
        private readonly IParkingCategoryData _data;
        private readonly IMapper _mapper;
        public ParkingCategoryBusiness(IParkingCategoryData data, IMapper mapper)
            : base(data, mapper)
        {
            _data = data;
            _mapper = mapper;
        }

        public override async Task<ParkingCategoryDto> Save(ParkingCategoryDto dto)
        {
            try
            {
                Validations.ValidateDto(dto, "Code","Name");
                if(dto.Name.Length > 50)
                    throw new ArgumentException("El nombre no puede contener mas de 50 caracteres.");

                dto.Asset = true;

                BaseModel entity = _mapper.Map<ParkingCategory>(dto);
                entity = await _data.Save((ParkingCategory)entity);

                return _mapper.Map<ParkingCategoryDto>(entity);
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

        public override async Task Update(ParkingCategoryDto dto)
        {
            try
            {
                Validations.ValidateDto(dto, "Id","Code", "Name");

                if (dto.Id <= 0)
                    throw new ArgumentException("No ha seleccioando ninguna categoría.");

                var parkingCategoryExistente = await _data.GetById(dto.Id);
                if (parkingCategoryExistente == null)
                    throw new InvalidOperationException($"Seleccone una categoria de parqueadero valida.");

                // 🔹 Actualizar entidad
                BaseModel entity = _mapper.Map<ParkingCategory>(dto);
                await _data.Update((ParkingCategory)entity);
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
