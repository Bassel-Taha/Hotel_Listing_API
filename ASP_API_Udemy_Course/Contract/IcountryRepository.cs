using ASP_API_Udemy_Course.Contract;
using ASP_API_Udemy_Course.data;

public interface IcountryRepository : IGenericRebository<Country>
{
    Task<Country> GetDetails(int Id);
    
}
