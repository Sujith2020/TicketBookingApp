using System.ComponentModel.DataAnnotations;

namespace Identity.Models
{
    public class Customer
    {
        [Key]
        public int CustId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
