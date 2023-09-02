using Microsoft.EntityFrameworkCore;

namespace AppControllers;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public object? this[string propertyName]
    {
        get => GetType().GetProperty(propertyName)?.GetValue(this, null);
        set => GetType().GetProperty(propertyName)?.SetValue(this, value, null);
    }
}