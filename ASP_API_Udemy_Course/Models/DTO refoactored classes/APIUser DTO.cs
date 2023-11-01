using ASP_API_Udemy_Course.Contract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using System.ComponentModel.DataAnnotations;

namespace ASP_API_Udemy_Course.Models.DTO_refoactored_classes
{
    public class APIUser_DTO : LoginDTO
    {
        [Required]
        public string  FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
    }
    

}
