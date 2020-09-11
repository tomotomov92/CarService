namespace BusinessLogic.DTOs
{
    public class ClientDTO : CredentialDTO, IBaseDTO
    {
        public bool Archived { get; set; }

        public static ClientDTO FromCredentialDTO(CredentialDTO dto)
        {
            return new ClientDTO
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                EmailAddress = dto.EmailAddress,
                Password = dto.HashedPassword,
            };
        }
    }
}
