namespace BusinessLogic.DTOs
{
    public class ClientDTO : IBaseDTO
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
