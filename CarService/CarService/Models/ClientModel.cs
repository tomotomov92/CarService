using BusinessLogic.DTOs;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CarService.Models
{
    public class ClientModel : CredentialModel
    {
        [DisplayName("Client Full Name")]
        public string FullName => $"{LastName}, {FirstName}";

        public static ClientModel FromDto(ClientDTO dto)
        {
            return new ClientModel
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                EmailAddress = dto.EmailAddress,
                Password = dto.Password,
                RepeatPassword = dto.RepeatPassword,
                RequirePasswordChange = dto.RequirePasswordChange,
                Activated = dto.Activated,
                Archived = dto.Archived,
                ErrorMessage = dto.ErrorMessage,
            };
        }

        public static IEnumerable<ClientModel> FromDtos(IEnumerable<ClientDTO> dtos)
        {
            return dtos.Select(dto => new ClientModel
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                EmailAddress = dto.EmailAddress,
                Password = dto.Password,
                RepeatPassword = dto.RepeatPassword,
                RequirePasswordChange = dto.RequirePasswordChange,
                Activated = dto.Activated,
                Archived = dto.Archived,
                ErrorMessage = dto.ErrorMessage,
            });
        }

        public static ClientDTO ToDto(ClientModel model)
        {
            return new ClientDTO
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailAddress = model.EmailAddress,
                Password = model.Password,
                RepeatPassword = model.RepeatPassword,
                RequirePasswordChange = model.RequirePasswordChange,
                Activated = model.Activated,
                Archived = model.Archived,
                ErrorMessage = model.ErrorMessage,
            };
        }
    }
}