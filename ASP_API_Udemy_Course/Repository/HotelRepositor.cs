using ASP_API_Udemy_Course.Models.data;
using ASP_API_Udemy_Course.Repository;
using AutoMapper;

public class HotelRepositor : GenericRepository<Hotel>, IhotelRepository
{
    public HotelRepositor(Hotel_Listing_DB_Context context , IMapper mapper) : base(context , mapper)
    {

    }
    
}