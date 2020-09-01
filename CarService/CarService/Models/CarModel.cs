using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CarService.Models
{
    public class CarModel
    {
        public int Id { get; set; }

        [Required, BindProperty, DisplayName("Client")]
        public int ClientId { get; set; }

        public ClientModel Client { get; set; }

        [Required, BindProperty, DisplayName("Car Brand")]
        public int CarBrandId { get; set; }

        public CarBrandModel CarBrand { get; set; }

        [Required, DisplayName("License Plate")]
        public string LicensePlate { get; set; }

        [Required, DisplayName("Mileage")]
        public int Mileage { get; set; }

        public bool Archived { get; set; }

        public SelectList CarBrandOptions { get; set; }

        public SelectList ClientOptions { get; set; }

        public static CarModel FromDto(CarDTO dto)
        {
            return new CarModel
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

        public static IEnumerable<CarModel> FromDtos(IEnumerable<CarDTO> dtos)
        {
            return dtos.Select(dto => new CarModel
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

        public static CarDTO ToDto(CarModel model)
        {
            return new CarDTO
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
