using AppControllers;
using Microsoft.EntityFrameworkCore;

namespace WebAPIUniversal.Models;

public class MyContext : AppDbContext
{
    public MyContext(DbContextOptions<MyContext> options) : base(options)
    {
    }

    public DbSet<Person> Person => Set<Person>();
    public DbSet<Device> Device => Set<Device>();
}