using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ASP_API_Udemy_Course.Models.model_data;
using ASP_API_Udemy_Course.Models.data;
using ASP_API_Udemy_Course.Models.automapping_data_for_security.county;
using System.Drawing;
using ASP_API_Udemy_Course.Repository;
using Microsoft.AspNetCore.Authorization;

namespace ASP_API_Udemy_Course.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly IcountryRepository _countryRepositor;
        private readonly IMapper _mapper;
        private readonly ILogger<CountriesController> _logger;

        public CountriesController(IcountryRepository countryRepositor, IMapper mapper , ILogger<CountriesController> logger)
        {
            this._countryRepositor = countryRepositor;
            this._mapper = mapper;
            this._logger = logger;
        }

        // GET: api/Countries
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GetCountryDTO>>> Getcountries()
        {
            _logger.LogInformation($"getting all the countries in the DB usign {nameof(Getcountries)}");
          if (await _countryRepositor.GetAllasync() == null)
          {
                _logger.LogError($"error in the {nameof(Getcountries)} funstion because there is not countries in the data base");
              return NotFound();
          }
            var countries = await _countryRepositor.GetAllasync();
            var getcountries = _mapper.Map <List<GetCountryDTO>>(countries);
            return getcountries;
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<GetFullCountryDetailsDTO>> GetCountry(int id)
        {
            _logger.LogInformation($"getting the country usign {nameof(GetCountry)} by Id ");
          if (await _countryRepositor.GetAllasync() == null)
          {
                _logger.LogError($"error getting all the countries in the DB because there is no countries in the DB");

                return NotFound();
          }

            var country = await _countryRepositor.GetDetails(id);

            if (country == null)
            {
                _logger.LogError($"error getting country by ID using {nameof(GetCountry)} because there is no country by that ID");

                return NotFound();
            }
            var CountryFullDetails = _mapper.Map<GetFullCountryDetailsDTO>(country);

            return CountryFullDetails;
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> PutCountry(int id, UpdateCountryDTO updateCountryDTO)
        {
            _logger.LogInformation($"editing the data of a country in the DB by ID using {nameof(PutCountry)} function");
            var country = await _countryRepositor.Getasync(id);

            if (id != country.Id)
            {
                _logger.LogError($"error the country id given isnt the same as the country in the function {nameof(PutCountry)}");
                return BadRequest();
            }

            try
            {
                #region //when using entity framework to find an entity in the database, it will be tracked by the context and if the entity is modified, the context will know about it and will update the database when SaveChanges() is called.
            
                //_context.Entry(country).State = EntityState.Modified;
                 _mapper.Map(updateCountryDTO, country);
                await _countryRepositor.Updateasync(country);
                #endregion
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CountryExists(id))
                {
                    _logger.LogError($"error getting country by ID using {nameof(PutCountry)} because there is no country by that ID");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<Country>> PostCountry(CreateNewCountry createNewCountry)
        {
            _logger.LogInformation($"posting a new country to the data base using {nameof(PostCountry)}");
            if (await _countryRepositor.GetAllasync() == null)
            {
                return Problem("Entity set 'Hotel_Listing_DB_Context.countries'  is null.");
            }
            if (createNewCountry == null)
            {
                return BadRequest("Argument 'createNewCountry' is null.");
            }
            var country = _mapper.Map<Country>(createNewCountry);
            await _countryRepositor.Addasync(country);
            

            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            _logger.LogInformation($"deleting a country from the data base with all its hotels using {nameof(DeleteCountry)}");
            if (await _countryRepositor.GetAllasync() == null)
            {
                _logger.LogError($"there is no countries in the data base");
                return NotFound();
            }
            var country = await _countryRepositor.Getasync(id);
            if (country == null)
            {
                _logger.LogError($"there is not country in the data base with the given ID" );
                return NotFound();
            }

            await _countryRepositor.Deleteasync(country.Id);

            _logger.LogInformation($"the country {country.Name} was deleted from the data base");

            return NoContent();
        }

        private async Task<bool> CountryExists(int id)
        {
            return await _countryRepositor.Exists(id);
        }
    }
}
