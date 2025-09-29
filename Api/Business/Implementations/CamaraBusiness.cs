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
        public async Task<IEnumerable<CameraDto>> GetAllJoinAsync()
        {
            try
            {
                IEnumerable<CameraDto> entities = await _data.GetAllJoinAsync();
                if (!entities.Any()) throw new InvalidOperationException("No se encontraron camaras.");
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
                throw new Exception("Error al obtener las camaras .", ex);
            }
        }

        public override async Task<CameraDto> Save(CameraDto dto)
        {
            try
            {
                Validations.ValidateDto(dto, "Resolution", "Url", "ParkingId", "Name");

                dto.Resolution = dto.Resolution?.Trim();
                dto.Url = dto.Url?.Trim();
                dto.Name = dto.Name?.Trim();

                if (string.IsNullOrWhiteSpace(dto.Name))
                    throw new ArgumentException("El campo 'Name' es obligatorio.");
                if (dto.Name.Length < 2) throw new ArgumentException("El nombre debe tener al menos 2 caracteres.");
                if (dto.Name.Length > 100) throw new ArgumentException("El nombre no puede superar los 100 caracteres.");

                if (string.IsNullOrWhiteSpace(dto.Resolution)) throw new ArgumentException("El campo 'Resolution' es obligatorio.");
                if (dto.Resolution.Length < 3) throw new ArgumentException("La resolución debe tener al menos 3 caracteres.");
                if (dto.Resolution.Length > 50) throw new ArgumentException("La resolución no puede superar los 50 caracteres.");

                if (string.IsNullOrWhiteSpace(dto.Url)) throw new ArgumentException("El campo 'Url' es obligatorio.");
                if (dto.Url.Length > 250) throw new ArgumentException("La URL no puede superar los 250 caracteres.");
                if (!Uri.IsWellFormedUriString(dto.Url, UriKind.Absolute)) throw new ArgumentException("La URL proporcionada no es válida.");

                if (dto.ParkingId <= 0) throw new ArgumentException("Debe seleccionar un estacionamiento válido.");

                // Normalizar para comparación (por parking si quieres unicidad por estacionamiento)
                var nameNorm = dto.Name.ToUpperInvariant();
                var urlNorm = dto.Url.ToUpperInvariant();
                var parkingId = dto.ParkingId;

                bool existsName = false;
                bool existsUrl = false;

                try
                {
                    existsName = await _data.ExistsAsync(c =>
                        c.Name != null &&
                        c.Name.ToUpper() == nameNorm &&
                        c.ParkingId == parkingId &&
                        (c.IsDeleted == null || c.IsDeleted == false)
                    );

                    existsUrl = await _data.ExistsAsync(c =>
                        c.Url != null &&
                        c.Url.ToUpper() == urlNorm &&
                        c.ParkingId == parkingId &&
                        (c.IsDeleted == null || c.IsDeleted == false)
                    );
                }
                catch
                {
                    var all = await _data.GetAll() ?? Enumerable.Empty<Camera>();
                    existsName = all.Any(c =>
                        !string.IsNullOrWhiteSpace(c.Name) &&
                        c.ParkingId == parkingId &&
                        string.Equals(c.Name.Trim(), dto.Name, StringComparison.OrdinalIgnoreCase) &&
                        !(c.IsDeleted ?? false)
                    );
                    existsUrl = all.Any(c =>
                        !string.IsNullOrWhiteSpace(c.Url) &&
                        c.ParkingId == parkingId &&
                        string.Equals(c.Url.Trim(), dto.Url, StringComparison.OrdinalIgnoreCase) &&
                        !(c.IsDeleted ?? false)
                    );
                }

                if (existsName) throw new InvalidOperationException($"Ya existe una cámara con el nombre '{dto.Name}' en ese estacionamiento.");
                if (existsUrl) throw new InvalidOperationException($"Ya existe una cámara con la URL '{dto.Url}' en ese estacionamiento.");

                // Mapear y garantizar inserción (evitar que venga Id desde cliente)
                dto.Asset = true;
                dto.IsDeleted = dto.IsDeleted.GetValueOrDefault(false);
                var entity = _mapper.Map<Camera>(dto);

                // Fuerza nuevo insert
                entity.Id = 0;
                entity.Parking = null; // evitar problemas con el mapeo de la relación

                entity = await _data.Save(entity);

                return _mapper.Map<CameraDto>(entity);
            }
            catch (InvalidOperationException) { throw; }
            catch (ArgumentException) { throw; }
            catch (Exception ex)
            {
                throw new BusinessException("Error al registrar la cámara.", ex);
            }
        }

        public override async Task Update(CameraDto dto)
        {
            try
            {
                Validations.ValidateDto(dto, "Id", "Resolution", "Url", "ParkingId", "Name");

                if (dto.Id <= 0) throw new ArgumentException("El Id de la cámara no es válido.");

                var camaraExistente = await _data.GetById(dto.Id);
                if (camaraExistente == null) throw new InvalidOperationException($"No existe una cámara con Id {dto.Id}.");
                if (!camaraExistente.Asset) throw new InvalidOperationException("No se puede actualizar una cámara deshabilitada.");

                // Normalizar
                dto.Name = dto.Name?.Trim();
                dto.Resolution = dto.Resolution?.Trim();
                dto.Url = dto.Url?.Trim();

                if (string.IsNullOrWhiteSpace(dto.Name)) throw new ArgumentException("El campo 'Name' es obligatorio.");
                if (dto.Name.Length < 2) throw new ArgumentException("El nombre debe tener al menos 2 caracteres.");
                if (dto.Name.Length > 100) throw new ArgumentException("El nombre no puede superar los 100 caracteres.");

                if (string.IsNullOrWhiteSpace(dto.Resolution)) throw new ArgumentException("El campo 'Resolution' es obligatorio.");
                if (dto.Resolution.Length < 3) throw new ArgumentException("La resolución debe tener al menos 3 caracteres.");
                if (dto.Resolution.Length > 50) throw new ArgumentException("La resolución no puede superar los 50 caracteres.");

                if (string.IsNullOrWhiteSpace(dto.Url)) throw new ArgumentException("El campo 'Url' es obligatorio.");
                if (dto.Url.Length > 250) throw new ArgumentException("La URL no puede superar los 250 caracteres.");
                if (!Uri.IsWellFormedUriString(dto.Url, UriKind.Absolute)) throw new ArgumentException("La URL proporcionada no es válida.");

                if (dto.ParkingId <= 0) throw new ArgumentException("Debe seleccionar un estacionamiento válido.");

                var nameNorm = dto.Name.ToUpperInvariant();
                var urlNorm = dto.Url.ToUpperInvariant();
                var parkingId = dto.ParkingId;

                bool existsOtherName = false;
                bool existsOtherUrl = false;

                try
                {
                    existsOtherName = await _data.ExistsAsync(c =>
                        c.Name != null &&
                        c.Name.ToUpper() == nameNorm &&
                        c.ParkingId == parkingId &&
                        c.Id != dto.Id &&
                        (c.IsDeleted == null || c.IsDeleted == false)
                    );

                    existsOtherUrl = await _data.ExistsAsync(c =>
                        c.Url != null &&
                        c.Url.ToUpper() == urlNorm &&
                        c.ParkingId == parkingId &&
                        c.Id != dto.Id &&
                        (c.IsDeleted == null || c.IsDeleted == false)
                    );
                }
                catch
                {
                    var all = await _data.GetAll() ?? Enumerable.Empty<Camera>();
                    existsOtherName = all.Any(c =>
                        c.Id != dto.Id &&
                        c.ParkingId == parkingId &&
                        !string.IsNullOrWhiteSpace(c.Name) &&
                        string.Equals(c.Name.Trim(), dto.Name, StringComparison.OrdinalIgnoreCase) &&
                        !(c.IsDeleted ?? false)
                    );
                    existsOtherUrl = all.Any(c =>
                        c.Id != dto.Id &&
                        c.ParkingId == parkingId &&
                        !string.IsNullOrWhiteSpace(c.Url) &&
                        string.Equals(c.Url.Trim(), dto.Url, StringComparison.OrdinalIgnoreCase) &&
                        !(c.IsDeleted ?? false)
                    );
                }

                if (existsOtherName) throw new InvalidOperationException($"Ya existe otra cámara con el nombre '{dto.Name}' en ese estacionamiento.");
                if (existsOtherUrl) throw new InvalidOperationException($"Ya existe otra cámara con la URL '{dto.Url}' en ese estacionamiento.");

                // Mapear sobre la instancia trackeada
                _mapper.Map(dto, camaraExistente);

                await _data.Update(camaraExistente);
            }
            catch (InvalidOperationException) { throw; }
            catch (ArgumentException) { throw; }
            catch (Exception ex)
            {
                throw new BusinessException("Error al actualizar la cámara.", ex);
            }
        }






    }
}