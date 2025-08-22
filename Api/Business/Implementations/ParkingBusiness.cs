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
  
    public class ParkingBusiness : RepositoryBusiness<Parking, ParkingDto>, IParkingBusiness
    {
        private readonly IParkingData _data;
        private readonly IMapper _mapper;
        public ParkingBusiness(IParkingData data, IMapper mapper)
            : base(data, mapper)
        {
            _data = data;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ParkingDto>> GetAllJoinAsync()
        {
            try
            {
                IEnumerable<ParkingDto> entities = await _data.GetAllJoinAsync();
                if (!entities.Any()) throw new InvalidOperationException("No se encontraron los parqueaderos.");
                return entities;
            }
            catch (InvalidOperationException invEx)
            {
                throw new InvalidOperationException("error: ", invEx);
            }
            catch (ArgumentException argEx)
            {
                throw new ArgumentException("error: ", argEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los parqueaderos.", ex);
            }
        }
        public override async Task<ParkingDto> Save(ParkingDto dto)
        {
            try
            {
                Validations.ValidateDto(dto, "ParkingCategoryId");

                if (dto.ParkingCategoryId <= 0)
                    throw new ArgumentException("El campo ParkingCategoryId debe ser mayor a 0.");

                if (!string.IsNullOrWhiteSpace(dto.Location))
                {
                    dto.Location = dto.Location.Trim();
                    if (dto.Location.Length > 150)
                        throw new ArgumentException("El campo Location no puede tener más de 150 caracteres.");

                    if (!System.Text.RegularExpressions.Regex.IsMatch(dto.Location, @"^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\s\.,-]+$"))
                        throw new ArgumentException("El campo Location contiene caracteres no permitidos.");
                }

                if (!string.IsNullOrWhiteSpace(dto.Location))
                {
                    var locationExistente = await _data.ExistsAsync<Parking>(
                        x => x is Parking p && p.Location == dto.Location && p.Asset == true
                    );
                    if (locationExistente)
                        throw new InvalidOperationException($"Ya existe un parking registrado en la ubicación '{dto.Location}'.");
                }

                dto.Asset = true;

                BaseModel entity = _mapper.Map<Parking>(dto);
                entity = await _data.Save((Parking)entity);

                return _mapper.Map<ParkingDto>(entity);
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
                throw new BusinessException("Error al crear el registro de parking.", ex);
            }
        }

        public override async Task Update(ParkingDto dto)
        {
            try
            {
                Validations.ValidateDto(dto, "Id", "ParkingCategoryId");

                if (dto.Id <= 0)
                    throw new ArgumentException("El campo Id debe ser mayor a 0.");

                var parkingExistente = await _data.GetById(dto.Id);
                if (parkingExistente == null)
                    throw new InvalidOperationException($"No existe un parking con Id {dto.Id}.");

                if (!parkingExistente.Asset)
                    throw new InvalidOperationException("No se puede actualizar un registro de parking deshabilitado.");

                if (dto.ParkingCategoryId <= 0)
                    throw new ArgumentException("El campo ParkingCategoryId debe ser mayor a 0.");

                if (!string.IsNullOrWhiteSpace(dto.Location))
                {
                    dto.Location = dto.Location.Trim();
                    if (dto.Location.Length > 150)
                        throw new ArgumentException("El campo Location no puede tener más de 150 caracteres.");

                    if (!System.Text.RegularExpressions.Regex.IsMatch(dto.Location, @"^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\s\.,-]+$"))
                        throw new ArgumentException("El campo Location contiene caracteres no permitidos.");

                    // 🔹 Validar que no haya otro parking activo con la misma ubicación
                    var locationDuplicada = await _data.ExistsAsync<Parking>(
                        x => x is Parking p && p.Location == dto.Location && p.Id != dto.Id && p.Asset == true
                    );
                    if (locationDuplicada)
                        throw new InvalidOperationException($"Ya existe otro parking activo en la ubicación '{dto.Location}'.");
                }

                // 🔹 Actualizar entidad
                BaseModel entity = _mapper.Map<Parking>(dto);
                await _data.Update((Parking)entity);
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
                throw new BusinessException("Error al actualizar el registro de parking.", ex);
            }
        }


    }
}
