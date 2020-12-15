using System.ComponentModel.DataAnnotations;

namespace BookingServiceNS.Models
{
    public class Movie
    {
        [Key]
        public int MovieId { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public string Language { get; set; }
    }
}
