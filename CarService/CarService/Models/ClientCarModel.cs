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

        public ClientModel Client { get; set; }

        public int CarBrandId { get; set; }

        public CarBrandModel CarBrand { get; set; }

        [DisplayName("License Plate")]
        public string LicensePlate { get; set; }

        [DisplayName("Mileage")]
        public int Mileage { get; set; }

        public bool Archived { get; set; }

        public static ClientCarModel FromDto(ClientCarDTO dto)
        {
            return new ClientCarModel
            {
                Id = dto.Id,
                ClientId = dto.ClientId,
                Client = ClientModel.FromDto(dto.Client),
                CarBrandId = dto.CarBrandId,
                CarBrand = CarBrandModel.FromDto(dto.CarBrand),
                LicensePlate = dto.LicensePlate,
                Mileage = dto.Mileage,
                Archived = dto.Archived,
            };
        }

        public static IEnumerable<ClientCarModel> FromDtos(IEnumerable<ClientCarDTO> dtos)
        {
            return dtos.Select(dto => new ClientCarModel
            {
                Id = dto.Id,
                ClientId = dto.ClientId,
                Client = ClientModel.FromDto(dto.Client),
                CarBrandId = dto.CarBrandId,
                CarBrand = CarBrandModel.FromDto(dto.CarBrand),
                LicensePlate = dto.LicensePlate,
                Mileage = dto.Mileage,
                Archived = dto.Archived,
            });
        }

        public static ClientCarDTO ToDto(ClientCarModel model)
        {
            return new ClientCarDTO
            {
                Id = model.Id,
                ClientId = model.ClientId,
                Client = ClientModel.ToDto(model.Client),
                CarBrandId = model.CarBrandId,
                CarBrand = CarBrandModel.ToDto(model.CarBrand),
                LicensePlate = model.LicensePlate,
                Mileage = model.Mileage,
                Archived = model.Archived,
            };
        }
    }
}
