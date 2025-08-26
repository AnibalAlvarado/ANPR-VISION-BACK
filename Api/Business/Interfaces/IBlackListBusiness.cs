using Entity.Dtos;

using Entity.Models;
using System.Linq.Expressions;
using Utilities.Exceptions;


namespace Business.Interfaces
{
    public interface IBlackListBusiness : IRepositoryBusiness<BlackList, BlackListDto>
    {

    }
}
