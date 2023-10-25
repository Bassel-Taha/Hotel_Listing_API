using ASP_API_Udemy_Course.Contract;
using ASP_API_Udemy_Course.Models.data;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace ASP_API_Udemy_Course.Repository
{
    public class CountryRepositor : GenericRepository<Country>, IcountryRepository
    {
        private readonly Hotel_Listing_DB_Context _context;

        public CountryRepositor(Hotel_Listing_DB_Context context) : base(context)
        {
            _context = context;
        }

        public async Task<Country> GetDetails(int Id)
        {

           return await _context.countries.Include(x => x.Hotels).FirstOrDefaultAsync(x => x.Id == Id);

        }
    }   
}

    

