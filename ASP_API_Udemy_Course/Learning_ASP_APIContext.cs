using Microsoft.EntityFrameworkCore;

namespace ASP_API_Udemy_Course
{
    public class Learning_ASP_APIContext : DbContext
    {
        public Learning_ASP_APIContext(DbContextOptions options) : base(options)
        {

        }
    }
}
