using Microsoft.EntityFrameworkCore;

namespace VueCsharpTest;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<TheaterEndpoints.TheaterShow> Shows => Set<TheaterEndpoints.TheaterShow>();
}