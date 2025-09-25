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
                Validations.ValidateDto(dto, "Code", "Name");

                dto.Name = dto.Name?.Trim();
                dto.Code = dto.Code?.Trim().ToUpper();

                if (string.IsNullOrWhiteSpace(dto.Name))
                    throw new ArgumentException("El nombre es obligatorio.");
                if (dto.Name.Length > 50)
                    throw new ArgumentException("El nombre no puede contener más de 50 caracteres.");

                if (string.IsNullOrWhiteSpace(dto.Code))
                    throw new ArgumentException("El código es obligatorio.");

                //  Duplicado por NOMBRE (case-insensitive)
                var nameExists = await _data.ExistsAsync(pc =>
                    pc.Name.ToLower() == dto.Name.ToLower() &&
                    pc.Asset
                );
                if (nameExists)
                    throw new ArgumentException($"Ya existe una categoría de parqueadero con el nombre '{dto.Name}'.");

                // (Opcional)  Duplicado por CÓDIGO (case-insensitive)
                var codeExists = await _data.ExistsAsync(pc =>
                    pc.code.ToUpper() == dto.Code &&
                    pc.Asset
                );
                if (codeExists)
                    throw new ArgumentException($"Ya existe una categoría de parqueadero con el código '{dto.Code}'.");

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
                Validations.ValidateDto(dto, "Id", "Code", "Name");

                if (dto.Id <= 0)
                    throw new ArgumentException("No ha seleccionado ninguna categoría.");

                var current = await _data.GetById(dto.Id);
                if (current == null)
                    throw new InvalidOperationException("Seleccione una categoría de parqueadero válida.");
                if (!current.Asset)
                    throw new InvalidOperationException("No se puede actualizar una categoría deshabilitada.");

                dto.Name = dto.Name?.Trim();
                dto.Code = dto.Code?.Trim().ToUpper();

                if (string.IsNullOrWhiteSpace(dto.Name))
                    throw new ArgumentException("El nombre es obligatorio.");
                if (dto.Name.Length > 50)
                    throw new ArgumentException("El nombre no puede contener más de 50 caracteres.");

                if (string.IsNullOrWhiteSpace(dto.Code))
                    throw new ArgumentException("El código es obligatorio.");

                //  Duplicado por NOMBRE en otros (case-insensitive)
                var nameExistsOther = await _data.ExistsAsync(pc =>
                    pc.Name.ToLower() == dto.Name.ToLower() &&
                    pc.Id != dto.Id &&
                    pc.Asset
                );
                if (nameExistsOther)
                    throw new ArgumentException($"Ya existe otra categoría de parqueadero con el nombre '{dto.Name}'.");

                // (Opcional)  Duplicado por CÓDIGO en otros (case-insensitive)
                var codeExistsOther = await _data.ExistsAsync(pc =>
                    pc.code.ToUpper() == dto.Code &&
                    pc.Id != dto.Id &&
                    pc.Asset
                );
                if (codeExistsOther)
                    throw new ArgumentException($"Ya existe otra categoría de parqueadero con el código '{dto.Code}'.");

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
