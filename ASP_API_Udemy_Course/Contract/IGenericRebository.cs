using Microsoft.EntityFrameworkCore;

namespace ASP_API_Udemy_Course.Contract
{
    public interface IGenericRebository <T> where T : class
    {
        //getting all entities from the database table
         Task<List<T>> GetAllasync();
        //getting entity to the database table
         Task <T> Getasync (int? Id);
        //adding entity from the database table
        Task <T> Addasync (T entity);
        //updating entity from the database table
        Task  Updateasync (T entity);
        //deleting entity from the database table
        Task  Deleteasync (int Id);
        //if the entity exists in the database table
        Task <bool> Exists (int id);

    }
}
