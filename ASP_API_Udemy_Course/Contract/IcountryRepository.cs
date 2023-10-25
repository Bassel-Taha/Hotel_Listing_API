using ASP_API_Udemy_Course.Contract;
using ASP_API_Udemy_Course.Models.data;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

public interface IcountryRepository : IGenericRebository<Country>
{
    Task<Country> GetDetails(int Id);
    
}
