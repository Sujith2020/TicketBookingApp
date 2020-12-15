using System;
using System.ComponentModel.DataAnnotations;

namespace BookingServiceNS.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }
        [Required]
        public string UserName { get; set; }
        public DateTime BookingTime { get; set; }
        public int ShowId { get; set; }
        public int seats { get; set; }
    }
}
