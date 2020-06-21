using BusinessLogic.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.BLs.Interfaces
{
    public interface IBaseBL<T>
        where T : IBaseDTO
    {
        AppDb Db { get; }

        Task<T> CreateAsync(T dto);

        Task<T> UpdateAsync(T dto);

        T ReadById(int id);

        IEnumerable<T> ReadAll();

        IEnumerable<T> ReadActive();

        Task DeleteAsync(int id);
    }
}