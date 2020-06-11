using BusinessLogic.DTOs;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CarService.Models
{
    public class ClientModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [DisplayName("Client Full Name")]
        public string FullName => $"{LastName}, {FirstName}";

        public static ClientModel FromDto(ClientDTO dto)
        {
            return new ClientModel
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
            };
        }

        public static IEnumerable<ClientModel> FromDtos(IEnumerable<ClientDTO> dtos)
        {
            return dtos.Select(dto => new ClientModel
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
            });
        }

        public static ClientDTO ToDto(ClientModel model)
        {
            return new ClientDTO
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
            };
        }
    }
}