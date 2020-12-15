using BookingService.Tests;
using BookingServiceNS.Controllers;
using BookingServiceNS.Models;
using BookingServiceNS.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace BankingService.Tests
{
    public class BookingControllerTest
    {
        private BookingController _bookingcontroller;
        private BookingFakeService _bookingservice;
        private MovieFakeService _movieservice;


        public BookingControllerTest()
        {
            _bookingservice = new BookingFakeService();
            _movieservice = new MovieFakeService();
        }

        [Fact]
        public void Get_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            var mockRepo = new Mock<IBookingService>();
            mockRepo.Setup(repo => repo.GetBookings())
                    .Returns(_bookingservice.GetBookings());
            _bookingcontroller = new BookingController(mockRepo.Object);

            // Act
            var okResult = _bookingcontroller.GetBookings();

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result.Result);
        }

        [Fact]
        public void Get_WhenCalled_ReturnsAllItems()
        {
            // Arrange
            var mockRepo = new Mock<IBookingService>();
            mockRepo.Setup(repo => repo.GetBookings())
                    .Returns(_bookingservice.GetBookings());
            _bookingcontroller = new BookingController(mockRepo.Object);

            // Act
            var okResult = _bookingcontroller.GetBookings().Result.Result as OkObjectResult;

            // Assert
            var items = Assert.IsType<List<Booking>>(okResult.Value);
            Assert.Equal(4, items.Count);
        }

        [Fact]
        public void GetById_IDPassed_ReturnsNotFoundResult()
        {
            // Arrange
            var mockRepo = new Mock<IBookingService>();
            mockRepo.Setup(repo => repo.GetBookings())
                    .Returns(_bookingservice.GetBookings());
            _bookingcontroller = new BookingController(mockRepo.Object);

            // Act
            var notFoundResult = _bookingcontroller.GetBooking(123456);
            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult.Result.Result);
        }

        [Fact]
        public void GetById_ExistingID_ReturnsOkResult()
        {
            // Arrange
            var mockRepo = new Mock<IBookingService>();
            mockRepo.Setup(repo => repo.GetBookings())
                    .Returns(_bookingservice.GetBookings());
            _bookingcontroller = new BookingController(mockRepo.Object);

            // Act
            var okResult = _bookingcontroller.GetBooking(12);
            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result.Result);
        }

        [Fact]
        public void GetById_ExistingIDPassed_ReturnsRightItem()
        {
            // Arrange
            var mockRepo = new Mock<IBookingService>();
            mockRepo.Setup(repo => repo.GetBooking(20))
                    .Returns(_bookingservice.GetBooking(12));
            _bookingcontroller = new BookingController(mockRepo.Object);

            // Act
            var okResult = _bookingcontroller.GetBooking(20).Result.Result as OkObjectResult;
            // Assert
            Assert.IsType<Booking>(okResult.Value);
            Assert.Equal("UserName", (okResult.Value as Booking).UserName);
        }

        [Fact]
        public void Add_InvalidObjectPassed_ReturnsBadRequest()
        {
            //Arrange
            var booking = new Booking()
            {
                BookingId = 11,
                ShowId = 1,
                BookingTime = System.DateTime.Now,
                seats = 5
            };

            var mockRepo = new Mock<IBookingService>();
            mockRepo.Setup(repo => repo.PostBooking(booking))
                    .Returns(_bookingservice.PostBooking(booking));
            _bookingcontroller = new BookingController(mockRepo.Object);


            _bookingcontroller.ModelState.AddModelError("UserName", "Required");
            // Act
            var badResponse = _bookingcontroller.PostBooking(booking);
            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse.Result.Result);
        }

        [Fact]
        public void Add_ValidObjectPassed_ReturnsCreatedResponse()
        {
            //Arrange
            var booking = new Booking()
            {
                BookingId = 11,
                UserName = "Akash",
                ShowId = 1,
                BookingTime = System.DateTime.Now,
                seats = 5
            };

            var mockRepo = new Mock<IBookingService>();
            mockRepo.Setup(repo => repo.PostBooking(booking))
                    .Returns(_bookingservice.PostBooking(booking));
            _bookingcontroller = new BookingController(mockRepo.Object);

            // Act
            var createdResponse = _bookingcontroller.PostBooking(booking);

            // Assert
            Assert.IsType<CreatedAtActionResult>(createdResponse.Result.Result);
        }

        [Fact]
        public void Add_MoreTicketsBooked_ReturnsBadResponse()
        {
            //Arrange
            var booking = new Booking()
            {
                BookingId = 11,
                UserName = "Akash",
                ShowId = 123,
                BookingTime = System.DateTime.Now,
                seats = 6
            };

            var mockRepo = new Mock<IBookingService>();
            mockRepo.Setup(repo => repo.PostBooking(booking))
                    .Returns(_bookingservice.PostBooking(booking));
            var mockRepo_show = new Mock<IMovieService>();
            mockRepo_show.Setup(repo => repo.Getshows())
                    .Returns(_movieservice.Getshows());


            _bookingcontroller = new BookingController(mockRepo.Object, mockRepo_show.Object);

            // Act
            var badResponse = _bookingcontroller.PostBooking(booking);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse.Result.Result);
        }

    }
}
