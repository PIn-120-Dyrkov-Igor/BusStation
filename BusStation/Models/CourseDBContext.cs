using Microsoft.EntityFrameworkCore;

namespace BusStation.Models
{
    public class CourseDBContext : DbContext
    {
        public CourseDBContext(DbContextOptions<CourseDBContext> options) : base(options) {}
        public DbSet<Passanger> Passers { get; set; }
    }
}
