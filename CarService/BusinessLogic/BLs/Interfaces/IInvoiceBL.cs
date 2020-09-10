using BusinessLogic.DTOs;
using System.Collections.Generic;

namespace BusinessLogic.BLs.Interfaces
{
    public interface IInvoiceBL<T> : IBaseBL<T>
        where T : IBaseDTO
    {
        IEnumerable<T> ReadForClientId(int clientId);

        IEnumerable<T> ReadForCarId(int carId);

        IEnumerable<T> ReadForInspectionId(int inspectionId);
    }
}