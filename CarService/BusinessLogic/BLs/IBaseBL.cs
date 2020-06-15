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

        Task<T> UpdateAsync(T dto);

        T Get(int id);

        Task DeleteAsync(int id);

        Task<IEnumerable<T>> GetAllAsync();

        Task<IEnumerable<T>> GetAllActiveAsync();
    }
}