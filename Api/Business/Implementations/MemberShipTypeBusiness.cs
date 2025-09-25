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
  
    public class MemberShipTypeBusiness : RepositoryBusiness<MemberShipType, MemberShipTypeDto>, IMemberShipTypeBusiness
    {
        private readonly IMemberShipTypeData _data;
        private readonly IMapper _mapper;
        public MemberShipTypeBusiness(IMemberShipTypeData data, IMapper mapper)
            : base(data, mapper)
        {
            _data = data;
            _mapper = mapper;
        }
        public override async Task<MemberShipTypeDto> Save(MemberShipTypeDto dto)
        {
            try
            {
                Validations.ValidateDto(dto, "Description", "PriceBase", "DurationDaysBase");

                dto.Description = dto.Description?.Trim();

                if (string.IsNullOrWhiteSpace(dto.Description))
                    throw new ArgumentException("El campo Descripción es obligatorio.");
                if (dto.Description.Length < 3)
                    throw new ArgumentException("La descripción debe tener al menos 3 caracteres.");
                // (Opcional) límite superior:
                // if (dto.Description.Length > 100)
                //     throw new ArgumentException("La descripción no puede superar los 100 caracteres.");

                if (dto.PriceBase <= 0)
                    throw new ArgumentException("El campo PrecioBase debe ser mayor que 0.");

                if (dto.DurationDaysBase <= 0)
                    throw new ArgumentException("El campo DurationDaysBase debe ser mayor que 0.");

                // 🔍 Duplicado por descripción (case-insensitive) en registros activos
                var exists = await _data.ExistsAsync(x =>
                    x.Description.ToLower() == dto.Description!.ToLower() &&
                    x.Asset
                );
                if (exists)
                    throw new InvalidOperationException("Ya existe un tipo de membresía activo con la misma descripción.");

                dto.Asset = true;

                BaseModel entity = _mapper.Map<MemberShipType>(dto);
                entity = await _data.Save((MemberShipType)entity);

                return _mapper.Map<MemberShipTypeDto>(entity);
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
                throw new BusinessException("Error al registrar el tipo de membresía.", ex);
            }
        }

        // Pseudocódigo detallado para solucionar el error:
        // 1. Reemplazar la llamada a _data.GetByIdAsync<MemberShipType>(dto.Id) por _data.GetById(dto.Id), ya que GetById está definido en IRepositoryData<T>.
        // 2. Reemplazar la llamada a _data.ExistsAsync<MemberShipType>(...) por una consulta manual usando _data.GetAll() y LINQ, ya que ExistsAsync no está definido.
        // 3. Ajustar el método Update para usar estas alternativas.

        public override async Task Update(MemberShipTypeDto dto)
        {
            try
            {
                Validations.ValidateDto(dto, "Id", "Description", "PriceBase", "DurationDaysBase");

                if (dto.Id <= 0)
                    throw new ArgumentException("El campo Id debe ser mayor que 0.");

                var tipoExistente = await _data.GetById(dto.Id);
                if (tipoExistente == null)
                    throw new InvalidOperationException($"No existe un tipo de membresía con Id {dto.Id}.");

                if (!tipoExistente.Asset)
                    throw new InvalidOperationException("No se puede actualizar un tipo de membresía deshabilitado.");

                dto.Description = dto.Description?.Trim();

                if (string.IsNullOrWhiteSpace(dto.Description))
                    throw new ArgumentException("El campo Descripción es obligatorio.");
                if (dto.Description.Length < 3)
                    throw new ArgumentException("La descripción debe tener al menos 3 caracteres.");
                // (Opcional) límite superior:
                // if (dto.Description.Length > 100)
                //     throw new ArgumentException("La descripción no puede superar los 100 caracteres.");

                if (dto.PriceBase <= 0)
                    throw new ArgumentException("El campo PrecioBase debe ser mayor que 0.");

                if (dto.DurationDaysBase <= 0)
                    throw new ArgumentException("El campo DurationDaysBase debe ser mayor que 0.");

                //  Duplicado por descripción en otros registros activos (excluye el propio Id)
                var existsOther = await _data.ExistsAsync(x =>
                    x.Description.ToLower() == dto.Description!.ToLower() &&
                    x.Id != dto.Id &&
                    x.Asset
                );
                if (existsOther)
                    throw new InvalidOperationException("Ya existe otro tipo de membresía activo con la misma descripción.");

                BaseModel entity = _mapper.Map<MemberShipType>(dto);
                await _data.Update((MemberShipType)entity);
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
                throw new BusinessException("Error al actualizar el tipo de membresía.", ex);
            }
        }

    }
}
