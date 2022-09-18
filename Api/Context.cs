using Microsoft.EntityFrameworkCore;

public class Context : DbContext
{
    public Context(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Report> Reports => Set<Report>();
}
