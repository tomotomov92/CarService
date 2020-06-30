using BusinessLogic.DTOs;
using BusinessLogic.Enums;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CarService.Models
{
    public class CredentialModel
    {
        public int Id { get; set; }

        [Required, DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required, DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required, DisplayName("Email Address")]
        public string EmailAddress { get; set; }

        [Required, DisplayName("Password")]
        public string Password { get; set; }

        [Required, DisplayName("Repeat Password")]
        public string RepeatPassword { get; set; }

        [Required, DisplayName("Employee Hire Date"), DataType(DataType.Date)]
        public DateTime DateOfStart { get; set; }

        [Required, DisplayName("Employee Role")]
        public UserRoles UserRole { get; set; }

        public string ConfirmationToken { get; set; }

        public bool SuccessfulOperation { get; set; }

        public bool RequirePasswordChange { get; set; }

        public string ErrorMessage { get; set; }

        public static CredentialModel FromDto(CredentialDTO dto)
        {
            return new CredentialModel
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                EmailAddress = dto.EmailAddress,
                Password = dto.Password,
                RepeatPassword = dto.RepeatPassword,
                DateOfStart = dto.DateOfStart,
                UserRole = dto.UserRole,
                SuccessfulOperation = dto.SuccessfulOperation,
                RequirePasswordChange = dto.RequirePasswordChange,
                ErrorMessage = dto.ErrorMessage,
            };
        }

        public static CredentialDTO ToDto(CredentialModel model)
        {
            return new CredentialDTO
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailAddress = model.EmailAddress,
                Password = model.Password,
                RepeatPassword = model.RepeatPassword,
                UserRole = model.UserRole,
                DateOfStart = model.DateOfStart,
                SuccessfulOperation = model.SuccessfulOperation,
                RequirePasswordChange = model.RequirePasswordChange,
                ErrorMessage = model.ErrorMessage,
            };
        }
    }
}
