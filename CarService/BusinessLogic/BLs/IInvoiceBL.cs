using BusinessLogic.DTOs;
using System.Collections.Generic;

namespace BusinessLogic.BLs
{
    public interface IInvoiceBL<T> : IBaseBL<T>
        where T : IBaseDTO
    {
        IEnumerable<T> ReadForClientId(int clientId);
    }
}