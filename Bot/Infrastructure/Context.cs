using Microsoft.EntityFrameworkCore;

public class Context : DbContext
{
    public DbSet<Member> Members => Set<Member>();
}
