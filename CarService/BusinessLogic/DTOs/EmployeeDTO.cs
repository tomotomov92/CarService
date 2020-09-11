namespace BusinessLogic.DTOs
{
    public class EmployeeDTO : CredentialDTO, IBaseDTO
    {
        public EmployeeRoleDTO EmployeeRole { get; set; }

        public bool Archived { get; set; }

        public static EmployeeDTO FromCredentialDTO(CredentialDTO dto)
        {
            return new EmployeeDTO
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                EmailAddress = dto.EmailAddress,
                Password = dto.HashedPassword,
                DateOfStart = dto.DateOfStart,
                EmployeeRoleId = dto.EmployeeRoleId,
            };
        }
    }
}
