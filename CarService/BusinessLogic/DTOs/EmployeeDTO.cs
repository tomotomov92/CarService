namespace BusinessLogic.DTOs
{
    public class EmployeeDTO : CredentialDTO, IBaseDTO
    {
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
                EmployeeRole = dto.EmployeeRole,
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
