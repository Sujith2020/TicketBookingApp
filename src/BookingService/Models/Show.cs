using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingServiceNS.Models
{
    public class Show
    {
        [Key]
        public int ShowId { get; set; }

        [ForeignKey("Movie")]
        public int MovieId { get; set; }
        public Movie movie { get; set; }

        [ForeignKey("Multiplex")]
        public int MulId { get; set; }
        public Multiplex multiplex { get; set; }
        public int seats { get; set; }
        public DateTime ShowTime { get; set; }

    }
}
