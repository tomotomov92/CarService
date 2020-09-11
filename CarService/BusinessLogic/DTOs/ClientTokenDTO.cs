using System;

namespace BusinessLogic.DTOs
{
    public class ClientTokenDTO : IBaseDTO
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public string Token { get; set; }

        public DateTime ExpirationDate { get; set; }

        public bool IsValid { get; set; }

        public string EmailAddress { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailSubject { get; set; }

        public string EmailBody { get; set; }
    }
}
