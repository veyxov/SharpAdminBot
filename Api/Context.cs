using Microsoft.EntityFrameworkCore;

public class Context : DbContext
{
    public DbSet<Report> Reports => Set<Report>();
}
