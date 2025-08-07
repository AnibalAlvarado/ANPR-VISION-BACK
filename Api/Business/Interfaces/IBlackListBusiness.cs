using Entity.Dtos;

using Entity.Models;


namespace Business.Interfaces
{
    public interface IBlackListBusiness : IRepositoryBusiness<BlackList, BlackListDto>
    {

        public Task<int> CreateAsync(BlackList entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BlackList>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BlackList>> GetByDateAsync(DateTime restrictionDate)
        {
            throw new NotImplementedException();
        }

        public Task<BlackList> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BlackList>> GetByReasonAsync(string reason)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(BlackList entity)
        {
            throw new NotImplementedException();
        }
    }
}
