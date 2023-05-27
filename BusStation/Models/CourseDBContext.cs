using Microsoft.EntityFrameworkCore;

namespace BusStation.Models
{
    public class CourseDBContext : DbContext
    {
        public CourseDBContext(DbContextOptions<CourseDBContext> options) : base(options) {}
        public DbSet<Bus> Buses { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<DriversComposition> DriversCompositions { get; set; }
        public DbSet<Passanger> Passangers { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Stop> Stops { get; set; }
        public DbSet<StopList> StopLists { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<StopsList> StopsLists { get; set; }

    }
}
