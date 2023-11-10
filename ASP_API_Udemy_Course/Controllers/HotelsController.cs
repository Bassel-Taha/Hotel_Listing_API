using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASP_API_Udemy_Course.Models.data;
using AutoMapper;
using ASP_API_Udemy_Course.Models.automapping_data_for_security.Hotel;
using ASP_API_Udemy_Course.Models.refactoring_data_fro_security.Hotel;
using ASP_API_Udemy_Course.Models.model_data;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.Authorization;

namespace ASP_API_Udemy_Course.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IhotelRepository _hotelRepository;

        public HotelsController( IMapper mapper, IhotelRepository hotelRepository)
        {
           
            this._mapper = mapper;
            this._hotelRepository = hotelRepository;
        }

        

        // GET: api/Hotels
        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<GetHotelDTO>>> Gethotels()
        {
            var GetAllHotels = _mapper.Map<List<GetHotelDTO>>(await _hotelRepository.GetAllasync());
            return GetAllHotels;
        }

        // GET: api/Hotels/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<GetHotelDTO>> GetHotel(int id)
        {
            var hotel = await _hotelRepository.Getasync(id);

            if (hotel == null)
            {
                return NotFound();
            }
            var GetHotelByID = _mapper.Map<GetHotelDTO>(hotel);
            
            return GetHotelByID;
        }

        // PUT: api/Hotels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutHotel(int id, PutHotelDTO putHotelDTO)
        {
            var hotel = await _hotelRepository.Getasync(id);
            if (hotel == null)
            {
                return BadRequest();
            }

            //_context.Entry(_mapper.Map(putHotelDTO, hotel)).State = EntityState.Modified;

            try
            {

               await _hotelRepository.Updateasync(_mapper.Map(putHotelDTO, hotel));
                

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await HotelExists(id))
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

        // POST: api/Hotels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Hotel>> PostHotel(AddNewHotel AddNewHotel)
        {
            if (AddNewHotel == null)
            {
                return BadRequest("Argument 'AddNewHotel' is null.");
            }
            var hotel = _mapper.Map<Hotel>(AddNewHotel);
            await _hotelRepository.Addasync(hotel);

            return CreatedAtAction("GetHotel", new { id = hotel.Id }, AddNewHotel);
        }

        // DELETE: api/Hotels/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var hotel = await _hotelRepository.Getasync(id);
            if (hotel == null)
            {
                return NotFound();
            }

            await _hotelRepository.Deleteasync(id);

            return NoContent();
        }

        private async Task <bool> HotelExists(int id)
        {
            return await _hotelRepository.Exists(id);
        }
    }
}
