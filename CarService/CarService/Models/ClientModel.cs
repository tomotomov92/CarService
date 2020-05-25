using BusinessLogic.DTOs;

namespace CarService.Models
{
    public class ClientModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public static ClientModel FromDto(ClientDTO dto)
        {
            return new ClientModel
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
            };
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