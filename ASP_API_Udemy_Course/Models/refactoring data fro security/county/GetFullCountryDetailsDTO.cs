using ASP_API_Udemy_Course.Models.automapping_data_for_security.county;
using ASP_API_Udemy_Course.Models.automapping_data_for_security.Hotel;

namespace ASP_API_Udemy_Course.Models.model_data
{
    public class GetFullCountryDetailsDTO: BaseCountryDTO
    {
        public int Id { get; set; }

        public List<GetHotelDTO> Hotels { get; set; }
    }

    
}
