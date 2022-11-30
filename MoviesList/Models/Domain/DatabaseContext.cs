using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MoviesList.Models.Domain
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }
        public DbSet<Movie> movies { get; set; }
        public DbSet<MovieGenre> moviegenre { get; set; }
        public DbSet<Genre> genre { get; set; }


    }
}
