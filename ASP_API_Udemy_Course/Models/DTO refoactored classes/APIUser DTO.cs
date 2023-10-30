using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using System.ComponentModel.DataAnnotations;

namespace ASP_API_Udemy_Course.Models.DTO_refoactored_classes
{
    public class APIUser_DTO 
    {
        [Required]
        public string  FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        //public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string  Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(10, ErrorMessage = "the password should be between 6 and 10 characters", MinimumLength = 6)]
        public string PasswordHash { get; set; }
    }
    

}
