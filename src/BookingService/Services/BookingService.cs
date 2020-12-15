using BookingServiceNS.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingServiceNS.Services
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetBookings();
        Task<Booking> GetBooking(int BookingId);
        Task<int> PostBooking(Booking booking);
        Task<int> PutBooking(int id, Booking booking);
        Task<int> DeleteBooking(Booking booking);
        bool BookingExists(int id);
    }
    public class BookingService : IBookingService
    {
        private MovieDbContext _movieContext;

        public BookingService(MovieDbContext movieContext)
        {
            _movieContext = movieContext;
        }
        public bool BookingExists(int id)
        {
            return _movieContext.Bookings.Any(e => e.BookingId == id);
        }

        public async Task<int> DeleteBooking(Booking booking)
        {
            _movieContext.Bookings.Remove(booking);
            return await _movieContext.SaveChangesAsync();
        }

        public async Task<Booking> GetBooking(int BookingId)
        {
            var booking = await _movieContext.Bookings.FindAsync(BookingId);
            return booking;
        }

        public async Task<IEnumerable<Booking>> GetBookings()
        {
            var bookings = await _movieContext.Bookings.ToListAsync();
            return bookings;
        }

        public async Task<int> PostBooking(Booking booking)
        {
            var show = _movieContext.Shows.Where(x => x.ShowId == booking.ShowId).FirstOrDefault();
            _movieContext.Bookings.Add(booking);

            show.seats -= booking.seats;
            _movieContext.Entry(show).State = EntityState.Modified;

            return await _movieContext.SaveChangesAsync();

        }

        public async Task<int> PutBooking(int id, Booking booking)
        {
            var show = _movieContext.Shows.Where(x => x.ShowId == booking.ShowId).FirstOrDefault();

            show.seats -= booking.seats;
            _movieContext.Entry(show).State = EntityState.Modified;
            _movieContext.Entry(booking).State = EntityState.Modified;

            return await _movieContext.SaveChangesAsync();


        }
    }
}
