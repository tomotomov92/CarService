using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CarService.Models
{
    public class CredentialModel
    {
        [Required, DisplayName("Email Address")]
        public string EmailAddress { get; set; }

        [Required, DisplayName("Password")]
        public string Password { get; set; }

        [Required, DisplayName("Repeat Password")]
        public string RepeatPassword { get; set; }

        public bool SuccessfulLogin { get; set; }

        public string ErrorMessage { get; set; }
    }
}
