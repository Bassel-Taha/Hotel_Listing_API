using System.ComponentModel.DataAnnotations;

namespace ASP_API_Udemy_Course.Models.automapping_data_for_security.Hotel
{
    public abstract class BaseHotelDTO
    {
        [Required]
        public string Name { get; set; }

        public float? Rating { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int CountryId { get; set; }
    }
}
