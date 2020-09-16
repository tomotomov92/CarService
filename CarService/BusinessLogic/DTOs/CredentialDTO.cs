using BusinessLogic.Enums;
using System;
using System.Security.Cryptography;

namespace BusinessLogic.DTOs
{
    public class CredentialDTO : IBaseDTO
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string Password { get; set; }

        public string RepeatPassword { get; set; }

        public string HashedPassword => HashPassword();

        public DateTime DateOfStart { get; set; }

        public int EmployeeRoleId { get; set; }

        public EmployeeRoleDTO EmployeeRole { get; set; }

        public bool Activated { get; set; }

        public bool Archived { get; set; }

        public UserRoles UserRole { get; set; }

        public bool SuccessfulOperation { get; set; }

        public bool RequirePasswordChange { get; set; }

        public string ErrorMessage { get; set; }

        private string HashPassword()
        {
            if (string.IsNullOrEmpty(Password))
            {
                return null;
            }

            var hash = new SHA256Managed();
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(Password);
            var hashBytes = hash.ComputeHash(plainTextBytes);

            //in this string you got the encrypted password
            return Convert.ToBase64String(hashBytes);
        }
    }
}
