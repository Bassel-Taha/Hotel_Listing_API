using ASP_API_Udemy_Course.Models.data;
using ASP_API_Udemy_Course.Repository;

public class HotelRepositor : GenericRepository<Hotel>, IhotelRepository
{
    public HotelRepositor(Hotel_Listing_DB_Context context) : base(context)
    {

    }
    
}