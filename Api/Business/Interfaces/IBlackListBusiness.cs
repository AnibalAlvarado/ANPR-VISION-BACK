using Entity.Dtos;

using Entity.Models;


namespace Business.Interfaces
{
    public interface IBlackListBusiness
    {

        Task<IEnumerable<BlackListDto>> GetAllAsync();
        Task<BlackListDto> GetByIdAsync(int id);
        Task<BlackListDto> CreateAsync(BlackListDto dto);
        Task<BlackListDto> UpdateAsync(int id, BlackListDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
