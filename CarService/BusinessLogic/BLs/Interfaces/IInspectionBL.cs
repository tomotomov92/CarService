using BusinessLogic.DTOs;
using System.Collections.Generic;

namespace BusinessLogic.BLs.Interfaces
{
    public interface IInspectionBL<T> : IBaseBL<T>
        where T : IBaseDTO
    {
        IEnumerable<T> ReadForEmployeeId(int employeeId);

        IEnumerable<T> ReadForClientId(int clientId);

        IEnumerable<T> ReadForCarId(int carId);
    }
}