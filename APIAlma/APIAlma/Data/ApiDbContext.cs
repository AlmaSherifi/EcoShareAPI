using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using APIAlma.Models;

namespace APIAlma.Data
{
    public class ApiDbContext : IdentityDbContext<CustomUser>
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options)
            :base(options)
            {
            }

            public DbSet<Tool> Tools { get; set; }
            public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}