using Microsoft.EntityFrameworkCore;
namespace weddingplanner.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) { }
        
        // this is the variable we will use to connect to the MySQL table, Lizards
        public DbSet<User> users { get; set; }
        public DbSet<Wedding> weddings { get; set; }
        public DbSet<Association> associations { get; set; }
    }
}