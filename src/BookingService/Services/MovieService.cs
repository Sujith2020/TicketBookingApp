using BookingServiceNS.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingServiceNS.Services
{
    public interface IMovieService
    {
        Task<IEnumerable<Movie>> GetMovies();
        Task<IEnumerable<Show>> Getshows();
        Task<Movie> GetMovie(int MovieId);
        Task<int> PostMovie(Movie movie);
        Task<int> PutMovie(int id, Movie movie);
        Task<int> DeleteMovie(Movie movie);
        bool MovieExists(int id);
    }
    public class MovieService : IMovieService
    {
        private MovieDbContext _movieContext;

        public MovieService(MovieDbContext movieContext)
        {
            this._movieContext = movieContext;
        }

        public async Task<IEnumerable<Show>> Getshows()
        {
            var Shows = await _movieContext.Shows.ToListAsync();
            return Shows;
        }

        public async Task<IEnumerable<Movie>> GetMovies()
        {
            var Movies = await _movieContext.Movies.ToListAsync();
            return Movies;
        }

        public async Task<Movie> GetMovie(int MovieId)
        {
            var Movie = await _movieContext.Movies.FindAsync(MovieId);
            return Movie;
        }

        public async Task<int> PostMovie(Movie movie)
        {
            _movieContext.Movies.Add(movie);
            return await _movieContext.SaveChangesAsync();
        }

        public async Task<int> PutMovie(int id, Movie movie)
        {
            _movieContext.Entry(movie).State = EntityState.Modified;
            return await _movieContext.SaveChangesAsync();
        }

        public async Task<int> DeleteMovie(Movie movie)
        {
            _movieContext.Movies.Remove(movie);
            return await _movieContext.SaveChangesAsync();
        }

        public bool MovieExists(int id)
        {
            return _movieContext.Movies.Any(e => e.MovieId == id);
        }
    }
}
