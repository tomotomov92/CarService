namespace BusinessLogic.DTOs
{
    public class ClientDTO : CredentialDTO, IBaseDTO
    {
        public static ClientDTO FromCredentialDTO(CredentialDTO dto)
        {
            return new ClientDTO
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                EmailAddress = dto.EmailAddress,
                Password = dto.HashedPassword,
                Activated = dto.Activated,
                Archived = dto.Archived,
                UserRole = dto.UserRole,
                SuccessfulOperation = dto.SuccessfulOperation,
                RequirePasswordChange = dto.RequirePasswordChange,
                ErrorMessage = dto.ErrorMessage,
            };
        }
    }
}
