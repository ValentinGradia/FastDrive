using FastDrive.Models;
using Microsoft.EntityFrameworkCore;

namespace FastDrive.Data
{
    public class FastDriveContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        public FastDriveContext() { }

        public FastDriveContext(DbContextOptions<FastDriveContext> options) : base(options) { }
    }
}
