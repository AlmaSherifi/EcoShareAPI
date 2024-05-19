using APIAlma.Models;
using Microsoft.AspNetCore.Identity;


namespace APIAlma
{
    public class CustomUser : IdentityUser 
    {
        public ICollection<Tool> Tools { get; set; } = new List<Tool>();

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}

