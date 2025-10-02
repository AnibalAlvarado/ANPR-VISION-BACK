using AutoMapper;
using Business.Interfaces;
using Data.Implementations;
using Data.Interfaces;
using Entity.Dtos;
using Entity.Dtos.vehicle;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities.Exceptions;
using Utilities.Helpers.Validators;

namespace Business.Implementations
{

    public class VehicleBusiness : RepositoryBusiness<Vehicle, VehicleDto> ,IVehicleBusiness
    {
        private readonly IVehicleData _data;
        private readonly IMapper _mapper;
        private readonly IRegisteredVehiclesData _registeredVehicleData; // Data para RegisteredVehicles
        private readonly ISectorsData _sectorData; // Data para sectores y slots
        private readonly ISlotsData _slotsData;

        public VehicleBusiness(IVehicleData data, IMapper mapper, IRegisteredVehiclesData registeredVehicleData, ISectorsData sectorData, ISlotsData slotsData)
            : base(data, mapper)
        {
            _data = data;
            _mapper = mapper;
            _registeredVehicleData = registeredVehicleData;
            _sectorData = sectorData;
            _slotsData = slotsData;
        }
        public async Task<IEnumerable<VehicleDto>> GetAllJoinAsync()
        {
            try
            {
                IEnumerable<VehicleDto> entities = await _data.GetAllJoinAsync();
                if (!entities.Any()) throw new InvalidOperationException("No se encontraron vehiculos");
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
                throw new Exception("Error al obtener las vehiculos .", ex);
            }
        }
        // Método para registrar vehículo y asignar slot
        public async Task<RegisteredVehiclesDto> RegisterVehicleWithSlotAsync(int vehicleId)
        {
            // 1Obtener el vehículo existente
            var vehicle = await _data.GetById(vehicleId);
            if (vehicle == null)
                throw new Exception("Vehículo no encontrado.");

            //  Obtener sectores compatibles con el tipo de vehículo
            var validSectors = await _sectorData.GetSectorsByVehicleTypeAsync(vehicle.TypeVehicleId);

            //  Filtrar slots disponibles
            var availableSlots = new List<Slots>();

            foreach (var sector in validSectors)
            {
                foreach (var slot in sector.Slots)
                {
                    bool isOccupied = await _registeredVehicleData.AnyActiveRegisteredVehicleInSlotAsync(slot.Id);
                    if (!isOccupied && slot.IsAvailable) // Validamos IsAvailable
                    {
                        availableSlots.Add(slot);
                    }
                }
            }

            //  Validar que haya slots libres
            if (!availableSlots.Any())
            {
                throw new Exception("No hay slots disponibles para este tipo de vehículo.");
            }

            //  Seleccionar un slot aleatorio
            var random = new Random();
            var assignedSlot = availableSlots[random.Next(availableSlots.Count)];

            // 6. Marcar el slot como ocupado
            assignedSlot.IsAvailable = false;
            await _slotsData.Update(assignedSlot);


            //  Crear RegisteredVehicle
            var registeredVehicle = new RegisteredVehicles
            {
                VehicleId = vehicle.Id,
                SlotsId = assignedSlot.Id,
                EntryDate = DateTime.UtcNow,
                Asset = true
            };

            await _registeredVehicleData.Save(registeredVehicle);

            RegisteredVehiclesDto returnRegisteredVehicle = _mapper.Map<RegisteredVehiclesDto>(registeredVehicle);

            returnRegisteredVehicle.Slots = assignedSlot.Name;

            return returnRegisteredVehicle;
        }
        //public override async Task<VehicleDto> Save(VehicleDto dto)
        //{
        //    try
        //    {
        //        // 🔹 Limpieza de strings
        //        dto.Plate = dto.Plate?.Trim().ToUpper() ?? "";
        //        dto.Color = dto.Color?.Trim();

        //        // 🔹 Validación de campos obligatorios
        //        Validations.ValidateDto(dto, "Plate", "TypeVehicleId", "ClientId");

        //        if (string.IsNullOrWhiteSpace(dto.Plate))
        //            throw new ArgumentException("El campo 'Plate' es obligatorio.");

        //        // 🔹 Validación de placa (Colombia)
        //        var regexColombia = @"^([A-Z]{3}-\d{3}|[A-Z]{2}-\d{3}[A-Z])$";
        //        if (!System.Text.RegularExpressions.Regex.IsMatch(dto.Plate, regexColombia))
        //            throw new ArgumentException("La placa no tiene un formato válido según la normativa colombiana.");

        //        // 🔹 Validación Color (opcional)
        //        if (!string.IsNullOrWhiteSpace(dto.Color) && dto.Color.Length > 30)
        //            throw new ArgumentException("El color no puede superar los 30 caracteres.");

        //        // 🔹 Validación TypeVehicleId
        //        if (dto.TypeVehicleId <= 0)
        //            throw new ArgumentException("Debe seleccionar un tipo de vehículo válido.");

        //        // 🔹 Validación ClientId
        //        if (dto.ClientId <= 0)
        //            throw new ArgumentException("Debe seleccionar un cliente válido.");

        //        // 🔹 Validar que la placa no exista ya en la BD
        //        var exists = await _data.ExistsAsync(v => v.Plate.ToUpper() == dto.Plate.ToUpper());
        //        if (exists)
        //            throw new ArgumentException($"Ya existe un vehículo registrado con la placa '{dto.Plate}'.");

        //        // 🔹 Guardar entidad
        //        dto.Asset = true;

        //        // 🔹 Mapear solo propiedades simples y IDs
        //        var config = new MapperConfiguration(cfg =>
        //        {
        //            cfg.CreateMap<VehicleDto, Vehicle>()
        //                .ForMember(dest => dest.Plate, opt => opt.MapFrom(src => src.Plate))
        //                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color))
        //                .ForMember(dest => dest.TypeVehicleId, opt => opt.MapFrom(src => src.TypeVehicleId))
        //                .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.ClientId))
        //                .ForMember(dest => dest.Client, opt => opt.Ignore())
        //                .ForMember(dest => dest.TypeVehicle, opt => opt.Ignore())
        //                .ForMember(dest => dest.RegisteredVehicles, opt => opt.Ignore())
        //                .ForMember(dest => dest.Memberships, opt => opt.Ignore());
        //        });
        //        var mapper = config.CreateMapper();
        //        Vehicle entity = mapper.Map<Vehicle>(dto);

        //        // 🔹 Guardar en la base de datos
        //        entity = await _data.Save(entity);

        //        // 🔹 Devolver DTO con solo IDs
        //        return new VehicleDto
        //        {
        //            Id = entity.Id,
        //            Plate = entity.Plate,
        //            Color = entity.Color,
        //            TypeVehicleId = entity.TypeVehicleId,
        //            ClientId = entity.ClientId,
        //            Asset = entity.Asset,
        //            IsDeleted = entity.IsDeleted
        //        };
        //    }
        //    catch (InvalidOperationException invOe)
        //    {
        //        throw new InvalidOperationException($"Error: {invOe.Message}", invOe);
        //    }
        //    catch (ArgumentException argEx)
        //    {
        //        throw new ArgumentException($"Error: {argEx.Message}");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new BusinessException("Error al registrar el vehículo.", ex);
        //    }
        //}

        public override async Task<VehicleDto> Save(VehicleDto dto)
        {
            try
            {
                // 🔹 Limpieza de strings
                dto.Plate = dto.Plate?.Trim().ToUpper() ?? "";
                dto.Color = dto.Color?.Trim();

                // 🔹 Validación de campos obligatorios
                Validations.ValidateDto(dto, "Plate", "TypeVehicleId", "ClientId");

                if (string.IsNullOrWhiteSpace(dto.Plate))
                    throw new ArgumentException("El campo 'Plate' es obligatorio.");

                // 🔹 Validación de placa según el tipo de vehículo en Colombia
                bool placaValida = false;
                if (dto.TypeVehicleId <= 0)
                    throw new ArgumentException("Debe seleccionar un tipo de vehículo válido.");

                switch (dto.TypeVehicleId)
                {
                    case 1: // Carros (particulares, oficiales, públicos)
                            // Ejemplo: ABC123
                        placaValida = Regex.IsMatch(dto.Plate, @"^[A-Z]{3}\d{3}$");
                        break;

                    case 4: // Motos
                            // Ejemplo: ABC12D
                        placaValida = Regex.IsMatch(dto.Plate, @"^[A-Z]{3}\d{2}[A-Z]$");
                        break;

                    case 3: // Diplomáticos
                            // Ejemplo: CD1234, OI1234, AT1234
                        placaValida = Regex.IsMatch(dto.Plate, @"^(CD|OI|AT)\d{3,4}$");
                        break;

                    case 2: // Oficiales especiales (Ejército, Policía, etc.)
                            // Ejemplo: EJC123, FAC456, POL789
                        placaValida = Regex.IsMatch(dto.Plate, @"^(EJC|FAC|POL)\d{3,4}$");
                        break;

                    default:
                        throw new ArgumentException("El tipo de vehículo no tiene validación definida.");
                }

                if (!placaValida)
                    throw new ArgumentException($"La placa '{dto.Plate}' no corresponde al formato válido para el tipo de vehículo seleccionado.");

                // 🔹 Validación Color (opcional)
                if (!string.IsNullOrWhiteSpace(dto.Color) && dto.Color.Length > 30)
                    throw new ArgumentException("El color no puede superar los 30 caracteres.");

                // 🔹 Validación TypeVehicleId
                if (dto.TypeVehicleId <= 0)
                    throw new ArgumentException("Debe seleccionar un tipo de vehículo válido.");

                // 🔹 Validación ClientId
                if (dto.ClientId <= 0)
                    throw new ArgumentException("Debe seleccionar un cliente válido.");

                // 🔹 Validar que la placa no exista ya en la BD
                var exists = await _data.ExistsAsync(v => v.Plate.ToUpper() == dto.Plate.ToUpper());
                if (exists)
                    throw new ArgumentException($"Ya existe un vehículo registrado con la placa '{dto.Plate}'.");

                // 🔹 Guardar entidad
                dto.Asset = true;

                Vehicle entity = _mapper.Map<Vehicle>(dto);

                // 🔹 Guardar en la base de datos
                entity = await _data.Save(entity);

                VehicleDto entityDto = _mapper.Map<VehicleDto>(entity);
                // 🔹 Devolver DTO con solo IDs
                return entityDto;
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
                throw new BusinessException("Error al registrar el vehículo.", ex);
            }
        }

        public async Task<RegisteredVehiclesDto?> GetActiveVehicleBySlotAsync(int slotId)
        {
            var registeredVehicle = await _data.GetActiveRegisteredVehicleBySlotAsync(slotId);

            if (registeredVehicle == null)
                return null;

            return new RegisteredVehiclesDto
            {
                Id = registeredVehicle.Id,
                SlotsId = registeredVehicle.SlotsId,
                VehicleId = registeredVehicle.Vehicle.Id,
                EntryDate = registeredVehicle.EntryDate
            };
        }
        public async Task<IEnumerable<VehicleClientListDto>> GetByClientIdWithPresenceAsync(int clientId)
        {
            if (clientId <= 0) throw new ArgumentException("El clientId es inválido.");
            try
            {
                var list = await _data.GetByClientIdWithPresenceAsync(clientId);
                return list; // lista vacía es válida
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los vehículos del cliente (con estado).", ex);
            }
        }

        public async Task<(VehicleDto Vehicle, RegisteredVehicles Registered)> SaveWithSlotAsync(VehicleDto dto)
        {
            using var transaction = await _data.BeginTransactionAsync();
            try
            {
                // 1. Guardar el vehículo (usa tu lógica actual de Save)
                var vehicle = await Save(dto);

                // 2. Intentar asignar slot
                var registered = await RegisterVehicleWithSlotAsync(vehicle.Id);

                // 3. Confirmar la transacción
                await transaction.CommitAsync();

                return (vehicle, registered);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


    }
}
