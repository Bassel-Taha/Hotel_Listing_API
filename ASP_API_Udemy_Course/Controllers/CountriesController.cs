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

namespace ASP_API_Udemy_Course.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly IcountryRepository _countryRepositor;
        private readonly IMapper _mapper;

        public CountriesController(IcountryRepository countryRepositor, IMapper mapper)
        {
            this._countryRepositor = countryRepositor;
            this._mapper = mapper;
        }

        // GET: api/Countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCountryDTO>>> Getcountries()
        {
          if (await _countryRepositor.GetAllasync() == null)
          {
              return NotFound();
          }
            var countries = await _countryRepositor.GetAllasync();
            var getcountries = _mapper.Map <List<GetCountryDTO>>(countries);
            return getcountries;
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetFullCountryDetailsDTO>> GetCountry(int id)
        {
          if (await _countryRepositor.GetAllasync() == null)
          {
              return NotFound();
          }

            var country = await _countryRepositor.GetDetails(id);

            if (country == null)
            {
                return NotFound();
            }
            var CountryFullDetails = _mapper.Map<GetFullCountryDetailsDTO>(country);

            return CountryFullDetails;
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, UpdateCountryDTO updateCountryDTO)
        {
            var country = await _countryRepositor.Getasync(id);

            if (id != country.Id)
            {
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
        public async Task<ActionResult<Country>> PostCountry(CreateNewCountry createNewCountry)
        {
            if (_countryRepositor.GetAllasync() == null)
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
        public async Task<IActionResult> DeleteCountry(int id)
        {
            if (await _countryRepositor.GetAllasync() == null)
            {
                return NotFound();
            }
            var country = await _countryRepositor.Getasync(id);
            if (country == null)
            {
                return NotFound();
            }

            await _countryRepositor.Deleteasync(country.Id);
            

            return NoContent();
        }

        private async Task<bool> CountryExists(int id)
        {
            return await _countryRepositor.Exists(id);
        }
    }
}
