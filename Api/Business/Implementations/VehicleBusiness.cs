using AutoMapper;
using Business.Interfaces;
using Data.Implementations;
using Data.Interfaces;
using Entity.Dtos;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public async Task<RegisteredVehicles> RegisterVehicleWithSlotAsync(int vehicleId)
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
                EntryDate = DateTime.UtcNow
            };

            await _registeredVehicleData.Save(registeredVehicle);


            return registeredVehicle;
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

    }
}
