using ASP_API_Udemy_Course.Models.automapping_data_for_security.county;
using ASP_API_Udemy_Course.Models.automapping_data_for_security.Hotel;
using ASP_API_Udemy_Course.Models.data;
using ASP_API_Udemy_Course.Models.model_data;
using AutoMapper;


// automapper configration

namespace ASP_API_Udemy_Course.AutoMapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Country, CreateNewCountry>().ReverseMap();
            CreateMap<Country, GetCountryDTO>().ReverseMap();
            CreateMap<Country, GetFullCountryDetailsDTO>().ReverseMap();
            CreateMap<Country, UpdateCountryDTO>().ReverseMap();


            CreateMap<Hotel, GetHotelDTO>().ReverseMap();

        }
    }
}
