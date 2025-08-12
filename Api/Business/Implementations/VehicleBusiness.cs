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

namespace Business.Implementations
{

    public class VehicleBusiness : IVehicleBusiness
    {
        private readonly IVehicleData _vehicleData;
        private readonly IMapper _mapper;

        public VehicleBusiness(IVehicleData vehicleData, IMapper mapper)
        {
            _vehicleData = vehicleData;
            _mapper = mapper;
        }

        public async Task<VehicleDto> GetByIdAsync(int id)
        {
            var entity = await _vehicleData.GetByIdAsync(id);
            return _mapper.Map<VehicleDto>(entity);
        }

        public async Task<IEnumerable<VehicleDto>> GetAllAsync()
        {
            var entities = await _vehicleData.GetAllAsync();
            return _mapper.Map<IEnumerable<VehicleDto>>(entities);
        }

        public async Task<int> AddAsync(VehicleDto dto)
        {
            var entity = _mapper.Map<Vehicle>(dto);
            return await _vehicleData.AddAsync(entity);
        }

        public async Task UpdateAsync(int id, VehicleDto dto)
        {
            var entity = await _vehicleData.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Vehicle con Id {id} no encontrado");

            _mapper.Map(dto, entity);
            await _vehicleData.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _vehicleData.DeleteAsync(id);
        }

        public Task<VehicleDto> GetByPlateAsync(string plate)
        {
            throw new NotImplementedException();
        }

        Task<bool> IVehicleBusiness.UpdateAsync(int id, VehicleDto vehicleDto)
        {
            throw new NotImplementedException();
        }

        Task<bool> IVehicleBusiness.DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
