using AutoMapper;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dtos;
using Entity.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Implementations
{
    public class BlackListBusiness : IBlackListBusiness
    {
        private readonly IBlackListData _blackListData;
        private readonly IMapper _mapper;

        public BlackListBusiness(IBlackListData blackListData, IMapper mapper)
        {
            _blackListData = blackListData;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BlackListDto>> GetAllAsync()
        {
            var entities = await _blackListData.GetAllAsync();
            return _mapper.Map<IEnumerable<BlackListDto>>(entities);
        }

        public async Task<BlackListDto> GetByIdAsync(int id)
        {
            var entity = await _blackListData.GetByIdAsync(id);
            return _mapper.Map<BlackListDto>(entity);
        }

        public async Task<BlackListDto> CreateAsync(BlackListDto dto)
        {
            var entity = _mapper.Map<BlackList>(dto);
            var created = await _blackListData.CreateAsync(entity);
            return _mapper.Map<BlackListDto>(created);
        }

        public async Task<BlackListDto> UpdateAsync(int id, BlackListDto dto)
        {
            var existing = await _blackListData.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"BlackList con Id {id} no encontrada");

            _mapper.Map(dto, existing);
            var updated = await _blackListData.UpdateAsync(existing);
            return _mapper.Map<BlackListDto>(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _blackListData.DeleteAsync(id);
        }
    }
}
