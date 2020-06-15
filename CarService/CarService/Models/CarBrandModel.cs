using BusinessLogic.DTOs;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CarService.Models
{
    public class CarBrandModel
    {
        public int Id { get; set; }

        [Required, DisplayName("Car Brand")]
        public string BrandName { get; set; }

        public bool Archived { get; set; }

        public static CarBrandModel FromDto(CarBrandDTO dto)
        {
            return new CarBrandModel
            {
                Id = dto.Id,
                BrandName = dto.BrandName,
                Archived = dto.Archived,
            };
        }

        public static IEnumerable<CarBrandModel> FromDtos(IEnumerable<CarBrandDTO> dtos)
        {
            return dtos.Select(dto => new CarBrandModel
            {
                Id = dto.Id,
                BrandName = dto.BrandName,
                Archived = dto.Archived,
            });
        }

        public static CarBrandDTO ToDto(CarBrandModel model)
        {
            return new CarBrandDTO
            {
                Id = model.Id,
                BrandName = model.BrandName,
                Archived = model.Archived,
            };
        }
    }
}
