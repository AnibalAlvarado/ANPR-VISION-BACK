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

    public class CamaraBusiness : RepositoryBusiness<Camera, CameraDto>, ICamaraBusiness
    {
        private readonly ICamaraData _data;
        private readonly IMapper _mapper;
        public CamaraBusiness(ICamaraData data, IMapper mapper)
            : base(data, mapper)
        {
            _data = data;
            _mapper = mapper;
        }
        public override async Task<CameraDto> Save(CameraDto dto)
        {
            try
            {
           
                Validations.ValidateDto(dto, "Resolution", "Url", "ParkingId");

          
                if (string.IsNullOrWhiteSpace(dto.Resolution))
                    throw new ArgumentException("El campo 'Resolution' es obligatorio.");
                if (dto.Resolution.Length < 3)
                    throw new ArgumentException("La resolución debe tener al menos 3 caracteres.");
                if (dto.Resolution.Length > 50)
                    throw new ArgumentException("La resolución no puede superar los 50 caracteres.");

    
                if (string.IsNullOrWhiteSpace(dto.Url))
                    throw new ArgumentException("El campo 'Url' es obligatorio.");
                if (dto.Url.Length > 250)
                    throw new ArgumentException("La URL no puede superar los 250 caracteres.");
                if (!Uri.IsWellFormedUriString(dto.Url, UriKind.Absolute))
                    throw new ArgumentException("La URL proporcionada no es válida.");

       
                if (dto.ParkingId <= 0)
                    throw new ArgumentException("Debe seleccionar un estacionamiento válido.");

             
                // 🔹 Guardar entidad
                dto.Asset = true;
                BaseModel entity = _mapper.Map<Camera>(dto);
                entity = await _data.Save((Camera)entity);

                return _mapper.Map<CameraDto>(entity);
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
                throw new BusinessException("Error al registrar la cámara.", ex);
            }
        }


        public override async Task Update(CameraDto dto)
        {
            try
            {
                Validations.ValidateDto(dto, "Id", "Resolution", "Url", "ParkingId");

                if (dto.Id <= 0)
                    throw new ArgumentException("El Id de la cámara no es válido.");

                var camaraExistente = await _data.GetById(dto.Id);
                if (camaraExistente == null)
                    throw new InvalidOperationException($"No existe una cámara con Id {dto.Id}.");

                if (!camaraExistente.Asset)
                    throw new InvalidOperationException("No se puede actualizar una cámara deshabilitada.");

                if (string.IsNullOrWhiteSpace(dto.Resolution))
                    throw new ArgumentException("El campo 'Resolution' es obligatorio.");
                if (dto.Resolution.Length < 3)
                    throw new ArgumentException("La resolución debe tener al menos 3 caracteres.");
                if (dto.Resolution.Length > 50)
                    throw new ArgumentException("La resolución no puede superar los 50 caracteres.");

                if (string.IsNullOrWhiteSpace(dto.Url))
                    throw new ArgumentException("El campo 'Url' es obligatorio.");
                if (dto.Url.Length > 250)
                    throw new ArgumentException("La URL no puede superar los 250 caracteres.");
                if (!Uri.IsWellFormedUriString(dto.Url, UriKind.Absolute))
                    throw new ArgumentException("La URL proporcionada no es válida.");

                if (dto.ParkingId <= 0)
                    throw new ArgumentException("Debe seleccionar un estacionamiento válido.");

                // 🔹 Verificar duplicados correctamente
                var camaras = await _data.GetAll(null);
                var existeCamara = camaras.Any(
                    x => x.Resolution.Trim().ToLower() == dto.Resolution.Trim().ToLower() &&
                         x.Url.Trim().ToLower() == dto.Url.Trim().ToLower() &&
                         x.ParkingId == dto.ParkingId &&
                         x.Id != dto.Id &&
                         x.Asset == true
                );

                if (existeCamara)
                    throw new InvalidOperationException("Ya existe otra cámara con esta resolución y URL en el mismo estacionamiento.");

                BaseModel entity = _mapper.Map<Camera>(dto);
                await _data.Update((Camera)entity);
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
                throw new BusinessException("Error al actualizar la cámara.", ex);
            }
        }


    }
}