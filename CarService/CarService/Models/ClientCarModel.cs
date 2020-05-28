using BusinessLogic.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace CarService.Models
{
    public class ClientCarModel
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public int CarBrandId { get; set; }

        public int Mileage { get; set; }

        public static ClientCarModel FromDto(ClientCarDTO dto)
        {
            return new ClientCarModel
            {
                Id = dto.Id,
                ClientId = dto.ClientId,
                CarBrandId = dto.CarBrandId,
                Mileage = dto.Mileage,
            };
        }

        public static IEnumerable<ClientCarModel> FromDtos(IEnumerable<ClientCarDTO> dtos)
        {
            return dtos.Select(dto => new ClientCarModel
            {
                Id = dto.Id,
                ClientId = dto.ClientId,
                CarBrandId = dto.CarBrandId,
                Mileage = dto.Mileage,
            });
        }

        public static ClientCarDTO ToDto(ClientCarModel model)
        {
            return new ClientCarDTO
            {
                Id = model.Id,
                ClientId = model.ClientId,
                CarBrandId = model.CarBrandId,
                Mileage = model.Mileage,
            };
        }
    }
}
