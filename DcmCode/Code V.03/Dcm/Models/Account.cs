using System.ComponentModel.DataAnnotations;

namespace Dcm.Models
{
    public class Login
    {
        [Required(ErrorMessage = "E-mail is necessary.")]
        public string Email_Login { get; set; }

        [Required(ErrorMessage = "Password is necessary.")]
        public string Password_Login { get; set; }

        public bool RememberMe { get; set; }

    }

}
