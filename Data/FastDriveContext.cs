using FastDrive.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.IDUser);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Car)
                .WithMany(c => c.Bookings)
                .HasForeignKey(b => b.CarPatent);

        }
    }
}
