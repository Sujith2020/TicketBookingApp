using System.ComponentModel.DataAnnotations;

namespace BookingServiceNS.Models
{
    public class Multiplex
    {
        [Key]
        public int MulId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public int Seats { get; set; }
    }
}
