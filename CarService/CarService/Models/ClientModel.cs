using BusinessLogic.DTOs;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CarService.Models
{
    public class ClientModel
    {
        public int Id { get; set; }

        [Required, DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required, DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Client Full Name")]
        public string FullName => $"{LastName}, {FirstName}";

        [Required, DisplayName("Email Address")]
        public string EmailAddress { get; set; }

        [Required]
        public string Password { get; set; }

        public bool Archived { get; set; }

        public static ClientModel FromDto(ClientDTO dto)
        {
            return new ClientModel
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                EmailAddress = dto.EmailAddress,
                Password = dto.Password,
                Archived = dto.Archived,
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
                Archived = dto.Archived,
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
                Archived = model.Archived,
            };
        }
    }
}