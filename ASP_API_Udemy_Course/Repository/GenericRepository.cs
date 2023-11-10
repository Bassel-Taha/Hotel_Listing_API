using ASP_API_Udemy_Course.Contract;
using ASP_API_Udemy_Course.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;

namespace ASP_API_Udemy_Course.Repository
{
    public class GenericRepository<T> : IGenericRebository<T> where T : class
    {

        #region injecting the database context
        private readonly Hotel_Listing_DB_Context _context;
        private readonly IMapper _mapper;

        public GenericRepository( Hotel_Listing_DB_Context context , IMapper mapper)
        {
            _context =  context;
            this._mapper = mapper;
        }
        #endregion


        //posting the data to the database
        public async Task<T> Addasync(T entity)
        {
           await _context.AddAsync<T>(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        //deleting the data from the database
        public async Task Deleteasync(int Id)
        {
            var entity = await Getasync(Id);
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();

        }

        //checking if the entity exists in the database
        public async Task<bool> Exists(int id)
        {
            var entity = await Getasync(id);
            return entity != null;
        }

        //getting all the data from the database
        public async Task<List<T>> GetAllasync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<PageResult<TResult>> GetAllPagedResultsAsync<TResult>(QueryParameters queryParameters)
        {
            var TotalsSize = await _context.Set<T>().CountAsync();
            var entities = await _context.Set<T>()
                            .Skip(queryParameters.StartIndex)
                            .Take(queryParameters.PageSize)
                            .ProjectTo<TResult>(_mapper.ConfigurationProvider)
                            .ToListAsync();
            return new PageResult<TResult>
            {
                TotalCount = TotalsSize,
                PageNumber = queryParameters.PageNumber,
                RecordNumber = queryParameters.PageSize,
                Itims = entities
            };
        }
        //getting a specific entity from the database
        public async Task<T> Getasync(int? Id)
        {
            if (_context.countries == null)
            {
                return null;
            }
            return await _context.Set<T>().FindAsync(Id);
        }

        //updating "put" the data in the database 
        public async Task Updateasync( T entity)
        {
            //also can use Update function to modify the entity
            ///_context.Update<T>(entity);
            ///and the db gonna change the sntity state to modified then return it again
            
            _context.Entry<T>(entity).State = EntityState.Modified;

            await _context.SaveChangesAsync();

        }


       
    }
}
