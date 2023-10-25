using System.ComponentModel.DataAnnotations;

namespace ASP_API_Udemy_Course.Models.automapping_data_for_security.county
{
    public abstract class BaseCountryDTO
    {
        [Required]
        public string Name { get; set; }

        public string Shortname { get; set; }
    }
}
