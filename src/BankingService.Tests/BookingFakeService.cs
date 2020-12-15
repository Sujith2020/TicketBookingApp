using BookingServiceNS.Models;
using BookingServiceNS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingService.Tests
{
    class BookingFakeService : IBookingService
    {
        List<Booking> bookings = new List<Booking>()
        {
            new Booking{ BookingId=11, UserName = "sujith", ShowId = 1, BookingTime = System.DateTime.Now, seats = 5 },
            new Booking{ BookingId=12, UserName = "Aravind", ShowId = 1, BookingTime = System.DateTime.Now, seats = 4 },
            new Booking{ BookingId=13, UserName = "Somesh", ShowId = 1, BookingTime = System.DateTime.Now, seats = 10},
            new Booking{ BookingId=14, UserName = "Viswa", ShowId = 1, BookingTime = System.DateTime.Now, seats = 8 }
        };

        public bool BookingExists(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteBooking(Booking booking)
        {
            throw new NotImplementedException();
        }

        public async Task<Booking> GetBooking(int BookingId)
        {
            return bookings.Where(x => x.BookingId == BookingId).FirstOrDefault();
        }

        public async Task<IEnumerable<Booking>> GetBookings()
        {
            return bookings;
        }

        public async Task<int> PostBooking(Booking booking)
        {
            bookings.Add(booking);
            return 1;
        }

        public Task<int> PutBooking(int id, Booking booking)
        {
            throw new NotImplementedException();
        }
    }
}
