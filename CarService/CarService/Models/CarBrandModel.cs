using BusinessLogic.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace CarService.Models
{
    public class CarBrandModel
    {
        public int Id { get; set; }

        public string BrandName { get; set; }

        public static CarBrandModel FromDto(CarBrandDTO dto)
        {
            return new CarBrandModel
            {
                Id = dto.Id,
                BrandName = dto.BrandName,
            };
        }

        public static IEnumerable<CarBrandModel> FromDtos(IEnumerable<CarBrandDTO> dtos)
        {
            return dtos.Select(dto => new CarBrandModel
            {
                Id = dto.Id,
                BrandName = dto.BrandName,
            });
        }

        public static CarBrandDTO ToDto(CarBrandModel model)
        {
            return new CarBrandDTO
            {
                Id = model.Id,
                BrandName = model.BrandName,
            };
        }
    }
}
