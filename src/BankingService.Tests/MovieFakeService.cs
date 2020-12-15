using BookingServiceNS.Models;
using BookingServiceNS.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingService.Tests
{
    class MovieFakeService : IMovieService
    {
        List<Show> shows = new List<Show>()
        {
            new Show{  ShowId = 123, MovieId = 12, MulId = 22, seats = 25, ShowTime = System.DateTime.Now }
        };

        public Task<int> DeleteMovie(Movie movie)
        {
            throw new NotImplementedException();
        }

        public Task<Movie> GetMovie(int MovieId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Movie>> GetMovies()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Show>> Getshows()
        {
            return shows;
        }

        public bool MovieExists(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> PostMovie(Movie movie)
        {
            throw new NotImplementedException();
        }

        public Task<int> PutMovie(int id, Movie movie)
        {
            throw new NotImplementedException();
        }
    }
}
