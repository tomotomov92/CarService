using BusinessLogic.DTOs;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CarService.Models
{
    public class ClientCarModel
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        [DisplayName("Client First Name")]
        public string ClientFirstName { get; set; }

        [DisplayName("Client Last Name")]
        public string ClientLastName { get; set; }

        [DisplayName("Client Full Name")]
        public string ClientFullName => $"{ClientLastName}, {ClientFirstName}";

        public int CarBrandId { get; set; }

        [DisplayName("Car Brand")]
        public string CarBrandName { get; set; }

        [DisplayName("License Plate")]
        public string LicensePlate { get; set; }

        [DisplayName("Mileage")]
        public int Mileage { get; set; }

        public static ClientCarModel FromDto(ClientCarDTO dto)
        {
            return new ClientCarModel
            {
                Id = dto.Id,
                ClientId = dto.ClientId,
                ClientFirstName = dto.ClientFirstName,
                ClientLastName = dto.ClientLastName,
                CarBrandId = dto.CarBrandId,
                CarBrandName = dto.CarBrandName,
                LicensePlate = dto.LicensePlate,
                Mileage = dto.Mileage,
            };
        }

        public static IEnumerable<ClientCarModel> FromDtos(IEnumerable<ClientCarDTO> dtos)
        {
            return dtos.Select(dto => new ClientCarModel
            {
                Id = dto.Id,
                ClientId = dto.ClientId,
                ClientFirstName = dto.ClientFirstName,
                ClientLastName = dto.ClientLastName,
                CarBrandId = dto.CarBrandId,
                CarBrandName = dto.CarBrandName,
                LicensePlate = dto.LicensePlate,
                Mileage = dto.Mileage,
            });
        }

        public static ClientCarDTO ToDto(ClientCarModel model)
        {
            return new ClientCarDTO
            {
                Id = model.Id,
                ClientId = model.ClientId,
                ClientFirstName = model.ClientFirstName,
                ClientLastName = model.ClientLastName,
                CarBrandId = model.CarBrandId,
                CarBrandName = model.CarBrandName,
                LicensePlate = model.LicensePlate,
                Mileage = model.Mileage,
            };
        }
    }
}
