using System.ComponentModel.DataAnnotations;

namespace ASP_API_Udemy_Course.Contract
{
    public class LoginDTO 
    {
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(25, ErrorMessage = "the password should be between 6 and 25 characters", MinimumLength = 6)]
        public string PasswordHash { get; set; }
    }
}