using BusinessLogic.DTOs;
using System.Threading.Tasks;

namespace BusinessLogic.BLs.Interfaces
{
    public interface ICredentialBL<T> : IBaseBL<T>
        where T : IBaseDTO
    {
        Task<CredentialDTO> RegisterAsync(CredentialDTO dto);

        CredentialDTO LogIn(CredentialDTO dto);

        Task<bool> ChangeEmailAddressAsync(CredentialDTO dto);

        Task<bool> ChangePasswordAsync(CredentialDTO dto);

        Task<ClientTokenDTO> ForgottenPasswordAsync(CredentialDTO dto);
    }
}