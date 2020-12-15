using System.Collections.Generic;
using System.Threading.Tasks;
using BookingServiceNS.Models;
using BookingServiceNS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingServiceNM.Controllers
{
    [Route("api/movie")]
    [Authorize]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private IMovieService _movieService;

        public MovieController(IMovieService MovieService)
        {
            this._movieService = MovieService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            var Movies = await _movieService.GetMovies();
            return Ok(Movies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> Get(int id)
        {
            var movie = await _movieService.GetMovie(id);
            if (movie == null)
            {
                return NotFound();
            }
            return Ok(movie);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie([FromBody]Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var movid = await _movieService.PostMovie(movie);
            return StatusCode(StatusCodes.Status201Created);
            //return Ok(movie);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Movie>> PutMovie(int id, Movie movie)
        {
            if (id != movie.MovieId)
            {
                return BadRequest();
            }

            try
            {
                var movid = await _movieService.PutMovie(id, movie);
            }

            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
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
        public async Task<ActionResult<Movie>> DeleteMovie(int id)
        {
            var movie = await _movieService.GetMovie(id);
            if (movie == null)
            {
                return NotFound();
            }
            var movid = await _movieService.DeleteMovie(movie);
            return Ok();
        }

        private bool MovieExists(int id)
        {
            return _movieService.MovieExists(id);
        }
    }
}
