using ASP_API_Udemy_Course.Contract;
using ASP_API_Udemy_Course.data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace ASP_API_Udemy_Course.Repository
{
    public class CountryRepositor : GenericRepository<Country>, IcountryRepository
    {
        private readonly Hotel_Listing_DB_Context _context;
        private readonly IMapper mapper;

        public CountryRepositor(Hotel_Listing_DB_Context context , IMapper mapper) : base(context , mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        public async Task<Country> GetDetails(int Id)
        {

           return await _context.countries.Include(x => x.Hotels).FirstOrDefaultAsync(x => x.Id == Id);

        }
    }   
}

    

