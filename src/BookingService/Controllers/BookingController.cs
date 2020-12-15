using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingServiceNS.Services;
using BookingServiceNS.Models;
using Microsoft.AspNetCore.Authorization;

namespace BookingServiceNS.Controllers
{
    [Route("api/booking")]
    [Authorize]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private IBookingService _bookingService;
        private IMovieService _movieService;


        public BookingController(IMovieService movieService)
        {
            _movieService = movieService;
        }
        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }
        public BookingController(IBookingService bookingService, IMovieService movieService)
        {
            _bookingService = bookingService;
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            var Bookings = await _bookingService.GetBookings();
            return Ok(Bookings);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
            var booking = await _bookingService.GetBooking(id);
            if (booking == null)
            {
                return NotFound();
            }
            return Ok(booking);
        }

        [HttpPost]
        public async Task<ActionResult<Booking>> PostBooking([FromBody]Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var shows = await _movieService.Getshows();
            var show = shows.Where(x => x.ShowId == booking.ShowId).FirstOrDefault();
            if (show != null)
            {
                if (show.seats < booking.seats)
                {
                    return BadRequest($"{booking.seats} tickets are not available for the selected show. Please try with other show");
                }
                else if (booking.seats > 5)
                {
                    return BadRequest("More than 5 tickets cannot be selected. Please try again");
                }
            }


            var bookingsdone = await _bookingService.PostBooking(booking);

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Booking>> PutBooking(int id, Booking booking)
        {
            if (id != booking.BookingId)
            {
                return BadRequest();
            }
            try
            {

                var shows = await _movieService.Getshows();
                var show = shows.Where(x => x.ShowId == booking.ShowId).FirstOrDefault();

                if (show.seats < booking.seats)
                {
                    return BadRequest($"{booking.seats} tickets are not available for the selected show. Please try with other show");
                }
                else if (booking.seats > 5)
                {
                    return BadRequest("More than 5 tickets cannot be selected. Please try again");
                }

                var bookingsdone = await _bookingService.PutBooking(id, booking);


            }

            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
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

        [HttpDelete("{id}")]
        public async Task<ActionResult<Booking>> DeleteBooking(int id)
        {
            var booking = await _bookingService.GetBooking(id);
            if (booking == null)
            {
                return NotFound();
            }
            var bookingid = await _bookingService.DeleteBooking(booking);
            return Ok();
        }

        private bool BookingExists(int id)
        {
            return _bookingService.BookingExists(id);
        }
    }
}
