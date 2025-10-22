using AutoMapper;
using Business.Interfaces;
using Business.Interfaces.Operational;
using Business.Interfaces.Parameter;
using Data.Implementations;
using Data.Implementations.Operational;
using Data.Interfaces.Operational;
using Entity.Dtos.Dashboard;
using Entity.Dtos.Operational;
using Entity.Enums;
using Entity.Models.Operational;
using Entity.Models.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Exceptions;
using Utilities.Helpers.Validators;

namespace Business.Implementations.Operational
{
   
    public class RegisteredVehicleBusiness : RepositoryBusiness<RegisteredVehicles, RegisteredVehiclesDto>, IRegisteredVehicleBusiness
    {
        private readonly IRegisteredVehiclesData _data;
        private readonly IVehicleBusiness _vehicleBusiness;
        private readonly ISectorsBusiness _sectorsBusiness;
        public RegisteredVehicleBusiness(IRegisteredVehiclesData data, IMapper mapper, IVehicleBusiness vehicleBusiness, ISectorsBusiness sectorsBusiness)
            : base(data, mapper)
        {
            _data = data;
            _vehicleBusiness = vehicleBusiness;
            _sectorsBusiness = sectorsBusiness;
        }


        public async Task<IEnumerable<RegisteredVehiclesDto>> GetAllJoinAsync()
        {
            try
            {
                IEnumerable<RegisteredVehiclesDto> entities = await _data.GetAllJoinAsync();
                if (!entities.Any()) throw new InvalidOperationException("No se encontraron registros de vehiculos.");
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
                throw new Exception("Error al obtener las registros .", ex);
            }
        }

        // ---------- NUEVOS MÉTODOS ----------
        public async Task<int> GetTotalCurrentlyParkedByParkingAsync(int parkingId)
        {
            if (parkingId <= 0) throw new ArgumentException("parkingId inválido.");
            try
            {
                return await _data.GetTotalCurrentlyParkedByParkingAsync(parkingId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el total de vehículos estacionados por parking.", ex);
            }
        }

        public async Task<int> GetTotalCurrentlyParkedAsync()
        {
            try
            {
                return await _data.GetTotalCurrentlyParkedAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el total de vehículos estacionados (global).", ex);
            }
        }

        public Task<VehicleTypeDistributionDto> GetVehicleTypeDistributionGlobalAsync(bool includeZeros = true)
        => _data.GetVehicleTypeDistributionGlobalAsync(includeZeros);

        public Task<List<OccupancyItemDto>> GetSectorOccupancyByZoneAsync(int zoneId)
        => _data.GetSectorOccupancyByZoneAsync(zoneId);

        public async Task<IEnumerable<RegisteredVehiclesDto>> GetByParkingAsync(int parkingId)
        {
            try
            {
                if (parkingId <= 0)
                    throw new ArgumentException("Debe especificar un ID de parqueadero válido.");

                var data = await _data.GetByParkingAsync(parkingId);

                if (!data.Any())
                    throw new InvalidOperationException("No se encontraron registros para este parqueadero.");

                return data;
            }
            catch (ArgumentException argEx)
            {
                throw new ArgumentException($"Error: {argEx.Message}");
            }
            catch (InvalidOperationException invEx)
            {
                throw new InvalidOperationException($"Error: {invEx.Message}", invEx);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener los vehículos por parqueadero.", ex);
            }
        }

        // Método para registrar vehículo y asignar slot
        public async Task<RegisteredVehiclesDto> RegisterVehicleWithSlotAsync(int vehicleId, int parkingId)
        {
            // 1Obtener el vehículo existente
            VehicleDto vehicle = await _vehicleBusiness.GetById(vehicleId) ?? throw new Exception("Vehículo no encontrado.");

            //  Obtener sectores compatibles con el tipo de vehículo
            List<Sectors> validSectors = await _sectorsBusiness.GetSectorsByVehicleTypeAsync(vehicle.TypeVehicleId, parkingId);

            //  Filtrar slots disponibles
            List<Slots> availableSlots = new List<Slots>();

            foreach (Sectors sector in validSectors)
            {
                foreach (Slots slot in sector.Slots)
                {
                    bool isOccupied = await _data.AnyActiveRegisteredVehicleInSlotAsync(slot.Id);
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
            Random random = new();
            Slots assignedSlot = availableSlots[random.Next(availableSlots.Count)];

            // 6. Marcar el slot como ocupado
            assignedSlot.IsAvailable = false;
            await _slotsData.Update(assignedSlot);


            //  Crear RegisteredVehicle
            RegisteredVehicles registeredVehicle = new RegisteredVehicles
            {
                VehicleId = vehicle.Id,
                SlotsId = assignedSlot.Id,
                EntryDate = DateTime.UtcNow,
                Status = ERegisterStatus.In,
                Asset = true
            };

            await _registeredVehicleData.Save(registeredVehicle);

            RegisteredVehiclesDto returnRegisteredVehicle = _mapper.Map<RegisteredVehiclesDto>(registeredVehicle);

            returnRegisteredVehicle.Slots = assignedSlot.Name;

            return returnRegisteredVehicle;
        }

    }
}
