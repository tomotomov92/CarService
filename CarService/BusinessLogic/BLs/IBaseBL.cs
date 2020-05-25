using BusinessLogic.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.BLs
{
    public interface IBaseBL<T>
        where T : IBaseDTO
    {
        AppDb Db { get; set; }

        Task<T> AddAsync(T dto);

        Task<IEnumerable<T>> GetAllAsync();
    }
}