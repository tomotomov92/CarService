using BusinessLogic.DTOs;
using System.Collections.Generic;

namespace BusinessLogic.BLs
{
    public interface IInspectionBL<T> : IBaseBL<T>
        where T : IBaseDTO
    {
        IEnumerable<T> ReadForClientId(int clientId);

        IEnumerable<T> ReadForCarId(int carId);
    }
}