using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dtos;
using Entity.Models;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business.Implementations
{
    public class PersonParkingBusiness: RepositoryBusiness<PersonParking, PersonParkingDto>, IPersonParkingBusiness
    {
        private readonly IPersonParkingData _data;
        private readonly IMapper _mapper;
        private readonly ILogger<PersonParkingBusiness> _logger;
        public PersonParkingBusiness(IPersonParkingData data, IMapper mapper, ILogger<PersonParkingBusiness> logger)
            : base(data, mapper)
        {
            _data = data;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<PersonParkingDto>> GetAllJoinAsync()
        {
            var entities = await _data.GetAllJoinAsync();
            return _mapper.Map<IEnumerable<PersonParkingDto>>(entities);
        }


        // SAVE
        public override async Task<PersonParkingDto> Save(PersonParkingDto dto)
        {
            try
            {
                if (dto == null) throw new ArgumentException("Datos inválidos.");
                if (dto.PersonId <= 0) throw new ArgumentException("FormId inválido.");
                if (dto.ParkingId <= 0) throw new ArgumentException("ModuleId inválido.");

                // Comprobación de duplicado: incluir TODAS las filas (también IsDeleted = true)
                bool exists = false;
                try
                {
                    exists = await _data.ExistsAsync(r =>
                        r.PersonId == dto.PersonId &&
                        r.ParkingId == dto.ParkingId
                    );
                }
                catch (Exception exExists)
                {
                    _logger?.LogWarning(exExists, "ExistsAsync falló en Save, usando GetAll() como fallback.");
                    var all = await _data.GetAll() ?? Enumerable.Empty<PersonParking>();
                    exists = all.Any(r =>
                        r.PersonId == dto.PersonId && r.ParkingId == dto.ParkingId
                    );
                }

                if (exists)
                    throw new ArgumentException("Ya existe un registro con esa misma combinación person-parking.");

                dto.Asset = dto.Asset ?? true;
                dto.IsDeleted = dto.IsDeleted ?? false;

                var entity = _mapper.Map<PersonParking>(dto);
                var saved = await _data.Save(entity);

                return _mapper.Map<PersonParkingDto>(saved);
            }
            catch (ArgumentException) { throw; }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error al registrar PersonParking");
                throw new BusinessException("Error al registrar PersonParking.", ex);
            }
        }

        // UPDATE
        public override async Task Update(PersonParkingDto dto)
        {
            try
            {
                if (dto == null) throw new ArgumentException("Datos inválidos.");
                if (dto.Id <= 0) throw new ArgumentException("Id inválido.");
                if (dto.PersonId <= 0) throw new ArgumentException("PersonId inválido.");
                if (dto.ParkingId <= 0) throw new ArgumentException("ParkingId inválido.");

                var current = await _data.GetById(dto.Id);
                if (current == null)
                    throw new InvalidOperationException($"No existe el registro con Id {dto.Id}.");

                // Si no cambia la combinación ni otros campos relevantes -> salir
                bool pairChanged = current.PersonId != dto.PersonId || current.ParkingId != dto.ParkingId;
                bool otherChanged =
                    (dto.IsDeleted != null && dto.IsDeleted != current.IsDeleted) ||
                    (dto.Asset != null && dto.Asset.Value != current.Asset);
                if (!pairChanged && !otherChanged)
                    return;

                // Comprobar duplicado entre todas las filas (incluye IsDeleted = true). Excluir el propio Id.
                bool existsOther = false;
                try
                {
                    existsOther = await _data.ExistsAsync(r =>
                        r.Id != dto.Id &&
                        r.PersonId == dto.PersonId &&
                        r.ParkingId == dto.ParkingId
                    );
                }
                catch (Exception exExists)
                {
                    _logger?.LogWarning(exExists, "ExistsAsync falló en Update; usando GetAll() como fallback.");
                    var all = await _data.GetAll() ?? Enumerable.Empty<PersonParking>();
                    existsOther = all.Any(r =>
                        r.Id != dto.Id &&
                        r.PersonId == dto.PersonId &&
                        r.ParkingId == dto.ParkingId
                    );
                }

                if (existsOther)
                    throw new ArgumentException("Ya existe otro registro con esa misma combinación ");

                // Mapear y actualizar
                current.PersonId = dto.PersonId;
                current.ParkingId = dto.ParkingId;
                if (dto.IsDeleted != null) current.IsDeleted = dto.IsDeleted;
                if (dto.Asset != null) current.Asset = dto.Asset.Value;

                try
                {
                    await _data.Update(current);
                }
                catch (Microsoft.EntityFrameworkCore.DbUpdateException dbEx)
                {
                    _logger?.LogError(dbEx, "DbUpdateException en FormModule.Update - posible violación de unicidad");
                    throw new ArgumentException("No fue posible actualizar: ya existe otro registro con la misma combinación ");
                }
            }
            catch (ArgumentException) { throw; }
            catch (InvalidOperationException) { throw; }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error al actualizar");
                throw new BusinessException("Error al actualizar", ex);
            }
        }
        //obtener parqueaderos por persona asociada
        public async Task<IEnumerable<ParkingDto>> GetParkingsByPersonIdAsync(int personId)
        {
            var parkings = await _data.GetParkingsByPersonIdAsync(personId);
            return _mapper.Map<IEnumerable<ParkingDto>>(parkings);
        }

    }
}
