using System;

namespace BusinessLogic.DTOs
{
    public class TokenDTO : IBaseDTO
    {
        public int Id { get; set; }

        public int? ClientId { get; set; }

        public int? EmployeeId { get; set; }

        public string Token => GenerateToken();

        public DateTime ExpirationDate { get; set; }

        public bool IsValid { get; set; }

        public string EmailAddress { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailSubject { get; set; }

        public string EmailBody { get; set; }

        private string GenerateToken()
        {
            var rd = new Random();
            var stringLength = 50;
            var allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-.";
            char[] chars = new char[stringLength];

            for (int i = 0; i < stringLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }
    }
}
