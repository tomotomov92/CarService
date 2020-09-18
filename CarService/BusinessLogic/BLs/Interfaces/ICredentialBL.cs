using BusinessLogic.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.BLs.Interfaces
{
    public interface ICredentialBL<T> : IBaseBL<T>
        where T : IBaseDTO
    {
        string ConfirmAccountSQL { get; }
        string InsertTokenSQL { get; }
        string SelectTokenSQL { get; }
        string UpdateTokenSQL { get; }

        Task<CredentialDTO> RegisterAsync(CredentialDTO dto, string webRootPath, string base_url, bool selfRegistration);

        CredentialDTO LogIn(CredentialDTO dto);

        Task<bool> ChangeEmailAddressAsync(CredentialDTO dto);

        Task<bool> ChangePasswordAsync(CredentialDTO dto);

        Task<bool> ForgottenPasswordAsync(CredentialDTO dto, string webRootPath, string base_url);

        Task<bool> ConfirmAccountAsync(T dto, TokenDTO tokenDTO);

        T ReadByEmailAddress(string emailAddress);

        Task<TokenDTO> CreateTokenAsync(TokenDTO dto);

        IEnumerable<TokenDTO> ReadTokenByUserId(TokenDTO dto);

        IEnumerable<TokenDTO> ReadActiveTokenByUserId(TokenDTO dto);

        Task<TokenDTO> UpdateTokenAsync(TokenDTO dto);
    }
}